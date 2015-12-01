using UnityEngine;
using System.Collections;

public class FrequencyReader : MonoBehaviour {

    private AudioSource audioSource;

    private float[] frequencyData;
    private int sampleCount = 1024;
    private float maxFrequency;

    public float avg;
    public float currAvg;
    public float threshold = 0.001f;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        frequencyData = new float[sampleCount];
        maxFrequency = 44100/2;
        Debug.Log(maxFrequency);

        avg = AverageFrequency(0, maxFrequency);
        currAvg = avg;
        threshold = avg / 4;
	}
	
	// Update is called once per frame
	void Update () {
        if (audioSource.isPlaying)
        {
            currAvg = (avg + ReadFrequency(0, maxFrequency)) / 2;
            if (avg == 0.0f)
            {
                //avg = currAvg;
            }
            else
            {
                //avg = (avg + currAvg) / 2;
            }

            if (currAvg > avg + threshold)
            {
                Debug.Log("Above avg");
            }
            else if (currAvg < avg - threshold)
            {
                Debug.Log("below avg");
            }
        }
	}

    float ReadFrequency(float lowFreq, float highFreq)
    {
        audioSource.GetSpectrumData(frequencyData, 0, FFTWindow.BlackmanHarris);

        lowFreq = Mathf.Clamp(lowFreq, 20, maxFrequency);
        highFreq = Mathf.Clamp(highFreq, lowFreq, maxFrequency);

        int n1 = (int)Mathf.Floor(lowFreq * sampleCount / maxFrequency);
        int n2 = (int)Mathf.Floor(highFreq * sampleCount / maxFrequency);

        float sum = 0;
        for (int i = n1; i <= n2; i++)
        {
            sum += frequencyData[i];
        }
        return Mathf.Abs(sum / n2);
    }

    float AverageFrequency(float lowFreq, float highFreq)
    {
        Debug.Log(audioSource.clip.samples);
        float[] samples = new float[audioSource.clip.samples];
        audioSource.clip.GetData(samples, 0);

        lowFreq = Mathf.Clamp(lowFreq, 20, maxFrequency);
        highFreq = Mathf.Clamp(highFreq, lowFreq, maxFrequency);

        int n1 = (int)Mathf.Floor(lowFreq * audioSource.clip.samples / maxFrequency);
        int n2 = (int)Mathf.Floor(highFreq * audioSource.clip.samples / maxFrequency);

        float sum = 0;
        for (int i = n1; i <= n2; i++)
        {
            sum += samples[i];
        }

        return Mathf.Abs(sum / audioSource.clip.samples);
    }
}
