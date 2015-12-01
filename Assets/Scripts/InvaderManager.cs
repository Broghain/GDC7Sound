using UnityEngine;
using System.Collections;

public class InvaderManager : MonoBehaviour {

    public static InvaderManager instance;

    private Behaviour currentBehaviour;
    private Behaviour nextBehaviour;

    private float timer = 0;
    public float changeValue = 0;

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
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                currentBehaviour = nextBehaviour;
            }
        }
	}

    //All invaders check which behaviour they should apply in each Update cycle
    public Behaviour CheckBehaviour()
    {
        return currentBehaviour;
    }

    public void ChangeBehaviour(float value)
    {
        value = Mathf.Sign(value);
        if (value > 0 && nextBehaviour == Behaviour.Slow)
        {
            nextBehaviour = Behaviour.Fast;
            timer = 0.5f;
        }
        else if(value < 0 && nextBehaviour == Behaviour.Fast)
        {
            nextBehaviour = Behaviour.Slow;
            timer = 0.5f;
        }
    }
}

public enum Behaviour
{
    Fast,
    Slow,
    None
}
