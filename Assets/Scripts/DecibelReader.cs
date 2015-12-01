using UnityEngine;
using System.Collections;

public class DecibelReader : MonoBehaviour {

    AudioSource audio;

    private int sampleCount = 1024;
    private float refValue = 0.1f;
    private float theshold = 0.02f;
    private float rmsValue;
    private float dBValue;

    public float avgDBValue;
    private float prevAvgValue;

    private float[] samples;

    private float timer = 0;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        samples = new float[sampleCount];
        avgDBValue = 0;
        prevAvgValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (audio.isPlaying)
        {
            ReadAudio();
            avgDBValue = (avgDBValue + dBValue) / 2;
        }

        InvaderManager.instance.ChangeBehaviour(avgDBValue);
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

        if (dBValue < -160)
        {
            dBValue = -160; //clamp decibel value above -160
        }
    }
}
