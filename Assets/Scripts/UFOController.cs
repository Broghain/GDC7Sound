using UnityEngine;
using System.Collections;

public class UFOController : RhythmBehaviour {

    private RhythmManager rhythmManager;

    private Vector3 nextPosition;

    private Vector3 direction;

	// Use this for initialization
	void Start () {
        nextPosition = transform.position;

        if (transform.position.x < 0)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }

        rhythmManager = RhythmManager.instance;
        rhythmManager.AddRhythmEntity(this);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, nextPosition, 5 * Time.deltaTime);

        if (transform.position.x > 10 || transform.position.x < -10)
        {
            rhythmManager.RemoveRhythmEntity(this);
            Destroy(this.gameObject);
        }
	}

    public override void RhythmicUpdate()
    {
        nextPosition += direction;
    }

    public void Kill()
    {
        GameManager.instance.IncreaseScore(1000);
        rhythmManager.RemoveRhythmEntity(this);
        Destroy(this.gameObject);
    }
}
