using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//http://pastebin.com/myXiu97R source
public class AudioReader : MonoBehaviour {

    private AudioSource audioSource;

    private AutoCorrelator autoco;
    private int maxlag = 100; // (in frames) largest lag to track
    private float decay = 0.997f; // smoothing constant for running average

    private float[] spectrum;
    private int arraySize = 1024;
    private int samplingRate = 44100;

    private int nBand = 12; // number of bands
    private float[] spec; //previous spectrum
    private float[] onsets;
    private int now = 0;
    private int sinceLast = 0;
    private float framePeriod;
    private int colMax = 120;

    private int blipDelayLength = 16;
    private int[] blipDelay;

    private float[] scores;
    private float[] doBeat;

    private float alpha;

    [SerializeField]
    private float threshold = 0.1f;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        autoco = new AutoCorrelator(maxlag, decay, framePeriod, GetBandWidth());

        spectrum = new float[arraySize];
        spec = new float[nBand];
        for (int i = 0; i < nBand; i++)
        {
            spec[i] = 100f;
        }

        onsets = new float[colMax];
        scores = new float[colMax];
        doBeat = new float[colMax];
        blipDelay = new int[blipDelayLength];
	}
	
	// Update is called once per frame
	void Update () {
        ReadAudio();
	}

    void ReadAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            float[] averages = CalcAverages();

            float onset = 0;
            for (int i = 0; i < nBand; i++)
            {
                float specValue = Mathf.Max(-100.0f, 20.0f * Mathf.Log10(averages[i]) + 160); // dB value of this band

                specValue *= 0.025f;
                float dbInc = specValue - spec[i];
                spec[i] = specValue;
                onset += dbInc;
            }
            onsets[now] = onset;

            autoco.NewVal(onset);

            float aMax = 0.0f;
            int tempopd = 0;
            float[] autocoVals = new float[maxlag];
            for (int i = 0; i < maxlag; i++)
            {
                float autocoVal = Mathf.Sqrt(autoco.GetAutoCoVal(i));
                if (autocoVal > aMax)
                {
                    aMax = autocoVal;
                    tempopd = i;
                }

                autocoVals[maxlag - 1 - i] = autocoVal;
            }

            float scoreMax = -999999;
            int scoreMaxIndex = 0;
            alpha = 100 * threshold;
            for (int i = tempopd / 2; i < Mathf.Min(colMax, 2 * tempopd); i++)
            {
                float score = onset + scores[(now - i + colMax) % colMax] - alpha * Mathf.Pow(Mathf.Log((float)i/tempopd), 2);
                if (score > scoreMax)
                {
                    scoreMax = score;
                    scoreMaxIndex = i;
                }
            }
            scores[now] -= scoreMax;

            float scoreMin = scores[0];
            for (int i = 0; i < colMax; i++)
            {
                if (scores[i] < scoreMin)
                {
                    scoreMin = scores[i];
                }
            }
            for (int i = 0; i < colMax; i++)
            {
                scores[i] -= scoreMin;
            }

            scoreMax = scores[0];
            scoreMaxIndex = 0;
            for (int i = 0; i < colMax; i++)
            {
                if (scores[i] > scoreMax)
                {
                    scoreMax = scores[i];
                    scoreMaxIndex = i;
                }
            }

            doBeat[now] = 0;
            sinceLast++;
            if (scoreMax == now)
            {
                if (sinceLast > tempopd / 4)
                {
                    Debug.Log("Beat");
                }
                blipDelay[0] = 1;
                doBeat[now] = 1;
                sinceLast = 0;
            }

            now++;
            if (now == colMax)
            {
                now = 0;
            }
        }
    }

    private float[] CalcAverages()
    {
        float[] avgs = new float[12];
        for (int i = 0; i < 12; i++)
        {
            float avg = 0;
            int lowFreq;
            if (i == 0)
            {
                lowFreq = 0;
            }
            else
            {
                lowFreq = (int)((samplingRate / 2) / Mathf.Pow(2, 12 - i));
            }
            int hiFreq = (int)((samplingRate / 2) / Mathf.Pow(2, 11 - i));
            int lowBound = GetBound(lowFreq);
            int hiBound = GetBound(hiFreq);

            for (int j = lowBound; j <= hiBound; j++)
            {
                avg += spectrum[j];
            }

            avg /= (hiBound - lowBound + 1);
            avgs[i] = avg;
        }
        return avgs;
    }

    private int GetBound(float freq)
    {
        float bandWidth = GetBandWidth();
        if (freq < bandWidth / 2)
        {
            return 0;
        }
        if(freq > samplingRate / 2 - bandWidth / 2)
        {
            return arraySize / 2;
        }

        float fraction = freq / samplingRate;
        int i = (int)Mathf.Round(arraySize * fraction);
        return i;
    }

    private float GetBandWidth()
    {
        return (2f / (float)arraySize) * (samplingRate / 2);
    }

    private class AutoCorrelator
    {
        private int del_length;
        private float decay;
        private float[] delays;
        private float[] outputs;
        private int indx;

        private float[] bpms;
        private float[] rweight;
        private float wmidbpm = 120f;
        private float woctavewidth;

        public AutoCorrelator(int len, float alpha, float framePeriod, float bandwidth)
        {
            woctavewidth = bandwidth;
            decay = alpha;
            del_length = len;
            delays = new float[del_length];
            outputs = new float[del_length];
            indx = 0;

            // calculate a log-lag gaussian weighting function, to prefer tempi around 120 bpm
            bpms = new float[del_length];
            rweight = new float[del_length];
            for (int i = 0; i < del_length; ++i)
            {
                bpms[i] = 60.0f / (framePeriod * (float)i);
                //Debug.Log(bpms[i]);
                // weighting is Gaussian on log-BPM axis, centered at wmidbpm, SD = woctavewidth octaves
                rweight[i] = (float)System.Math.Exp(-0.5f * System.Math.Pow(System.Math.Log(bpms[i] / wmidbpm) / System.Math.Log(2.0f) / woctavewidth, 2.0f));
            }
        }

        public void NewVal(float val)
        {
            delays[indx] = val;

            // update running autocorrelator values
            for (int i = 0; i < del_length; ++i)
            {
                int delix = (indx - i + del_length) % del_length;
                outputs[i] += (1 - decay) * (delays[indx] * delays[delix] - outputs[i]);
            }

            if (++indx == del_length) indx = 0;
        }

        // read back the current autocorrelator value at a particular lag
        public float GetAutoCoVal(int del)
        {
            float blah = rweight[del] * outputs[del];
            return blah;
        }

        public float GetAvgBpm()
        {
            float sum = 0;
            for (int i = 0; i < bpms.Length; ++i)
            {
                sum += bpms[i];
            }
            return sum / del_length;
        }
    }
}
