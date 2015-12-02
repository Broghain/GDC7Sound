using UnityEngine;
using System.Collections;

public class SoundReader : MonoBehaviour {

    AudioSource audio;

    private int sampleCount = 1024;
    private int avgLength = 64;
    private float refValue = 0.1f;
    private float theshold = 0.02f;
    private float rmsValue;
    private float dBValue;
    private float pitchValue;

    public float avgDBValue;
    public float avgPitchValue;

    private float[] samples;
    private float[] spectrum;

    private float[] pitchValues;
    private int pitchIndex = 0;

    private float[] dBValues;
    private int dBIndex = 0;

    private float timer = 0;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        samples = new float[sampleCount];
        spectrum = new float[sampleCount];
        pitchValues = new float[avgLength];
        dBValues = new float[avgLength];
        avgDBValue = 0;
        avgPitchValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (audio.isPlaying)
        {
            ReadAudio();
            avgDBValue = GetAvgDB();
            avgPitchValue = GetAvgPitch();
        }

        InvaderManager.instance.ChangeBehaviour(avgDBValue, avgPitchValue);
	}

    void ReadAudio()
    {
        audio.GetOutputData(samples, 0); //Fill sample array
        float sum = 0f;
        for (int i = 0; i < sampleCount; i++)
        {
            sum += samples[i] * samples[i]; //sum is sum + squared sample value
        }
        rmsValue = Mathf.Sqrt(sum / sampleCount); //root value of squared sum
        dBValue = 20 * Mathf.Log10(rmsValue / refValue); //calculate decibel value

        if (dBValue < -20)
        {
            dBValue = -20; //clamp decibel value above -20
        }

        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris); //Fill array with spectrum data
        float maxValue = 0;
        int maxIndex = 0;
        for (int i = 0; i < sampleCount; i++) //Find max value in array
        {
            if (spectrum[i] > maxValue && spectrum[i] > theshold)
            {
                maxValue = spectrum[i];
                maxIndex = i; //store index of max value
            }
        }

        float freqIndex = maxIndex;
        if (maxIndex > 0 && maxIndex < sampleCount - 1) //if max value index is not first or last in array
        {
            float dL = spectrum[maxIndex - 1] / spectrum[maxIndex];
            float dR = spectrum[maxIndex + 1] / spectrum[maxIndex];
            freqIndex += 0.5f * (dR * dR - dL * dL); //interpolate with neighboring frequency values
        }
        pitchValue = freqIndex * (AudioSettings.outputSampleRate / 2) / sampleCount; //calculate average pitch
    }

    //calculate average of pitch averages
    private float GetAvgPitch()
    {
        pitchValues[pitchIndex] = pitchValue;

        float sum = 0;
        for (int i = 0; i < avgLength; i++)
        {
            sum += pitchValues[i];
        }

        pitchIndex++;
        if (pitchIndex >= avgLength)
        {
            pitchIndex = 0;
            return sum / avgLength;
        }
        return sum / pitchIndex;
    }

    //calculate average of decibel averages
    private float GetAvgDB()
    {
        dBValues[dBIndex] = dBValue;

        float sum = 0;
        for (int i = 0; i < avgLength; i++)
        {
            sum += dBValues[i];
        }
        
        dBIndex++;
        if (dBIndex >= avgLength)
        {
            dBIndex = 0;
            return sum / avgLength;
        }
        return sum / dBIndex;
    }
}
