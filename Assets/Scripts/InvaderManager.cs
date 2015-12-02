using UnityEngine;
using System.Collections;

public class InvaderManager : MonoBehaviour {

    public static InvaderManager instance;

    private Behaviour currentBehaviour;
    private Behaviour nextBehaviour;

    private float timer = 0;

    private float dBValue = 0;
    private float pitchValue = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        currentBehaviour = Behaviour.Slow;
	}
	
	// Update is called once per frame
	void Update () 
    {
        currentBehaviour = nextBehaviour;
	}

    //All invaders check which behaviour they should apply in each Update cycle
    public Behaviour CheckBehaviour()
    {
        return currentBehaviour;
    }

    public void ChangeBehaviour(float dBValue, float pitchValue)
    {
        this.dBValue = dBValue;
        this.pitchValue = pitchValue;
    }

    public float GetPitchValue()
    {
        return pitchValue;
    }

    public float GetDBValue()
    {
        return dBValue;
    }
}

public enum Behaviour
{
    Fast,
    Slow,
    None
}
