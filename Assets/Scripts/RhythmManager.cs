using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RhythmManager : MonoBehaviour, AudioProcessor.AudioCallbacks {

    public static RhythmManager instance;

    [SerializeField]
    private bool fakeRhythm;
    [SerializeField]
    private float fakeRhythmInterval = 0.5f;
    private float timer = 0.0f;

    [SerializeField]
    private List<RhythmBehaviour> entities = new List<RhythmBehaviour>();

    private AudioProcessor audioProcessor;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        audioProcessor = FindObjectOfType<AudioProcessor>();
        audioProcessor.addAudioCallback(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (fakeRhythm)
        {
            timer += Time.deltaTime;
            if (timer >= fakeRhythmInterval)
            {
                foreach (RhythmBehaviour entity in entities)
                {
                    if (entity != null)
                    {
                        entity.RhythmicUpdate();
                    }
                }
                timer = 0.0f;
            }
        }
	}

    public void AddRhythmEntity(RhythmBehaviour entity)
    {
        entities.Add(entity);
    }

    public void RemoveRhythmEntity(RhythmBehaviour entity)
    {
        entities.Remove(entity);
    }
    
    public void onOnbeatDetected()
    {
        foreach (RhythmBehaviour entity in entities)
        {
            if (entity != null)
            {
                entity.RhythmicUpdate();
            }
        }
    }

    public void onSpectrum(float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands
    }
}
