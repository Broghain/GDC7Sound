using UnityEngine;
using System.Collections;

public class PlayerController : RhythmBehaviour
{
    private RhythmManager rhythmManager;

    [SerializeField]
    private GameObject bulletPrefab;

    public float timer = 0.5f;

    private Vector3 nextPosition;

    private Vector3 startPosition;

	// Use this for initialization
	void Start () {
        nextPosition = transform.position;
        startPosition = transform.position;

        rhythmManager = RhythmManager.instance;
        rhythmManager.AddRhythmEntity(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.IsGameStarted())
        {
            transform.position = Vector3.Lerp(transform.position, nextPosition, 10 * Time.deltaTime);
        }
	}

    public override void RhythmicUpdate()
    {
        if (GameManager.instance.IsGameStarted())
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                nextPosition += Vector3.left;

            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                nextPosition += Vector3.right;
            }
            nextPosition.x = Mathf.Clamp(nextPosition.x, -7, 7);

            if (Input.GetKey(KeyCode.Z))
            {
                Instantiate(bulletPrefab, nextPosition + Vector3.up, Quaternion.identity);
            }
        }
    }

    public void Reset()
    {
        transform.position = startPosition;
        nextPosition = startPosition;
    }
}
