using UnityEngine;
using System.Collections;

public class BulletController : RhythmBehaviour {

    private RhythmManager rhythmManager;

    private float timer = 0.5f;

    private Vector3 nextPosition;

    [SerializeField]
    private Vector3 moveDirection = Vector3.up;

    [SerializeField]
    private GameObject explosionPrefab;

    private Vector3 bottomLeft;
    private Vector3 topRight;

	// Use this for initialization
	void Start () {
        nextPosition = transform.position;

        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        rhythmManager = RhythmManager.instance;
        rhythmManager.AddRhythmEntity(this);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, nextPosition, 10 * Time.deltaTime);

        if (transform.position.y > topRight.y || transform.position.y < bottomLeft.y)
        {
            rhythmManager.RemoveRhythmEntity(this);
            Destroy(this.gameObject);
        }
	}

    public override void RhythmicUpdate()
    {
        nextPosition = transform.position + moveDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        InvaderController invader = collider.GetComponent<InvaderController>();
        if (invader)
        {
            rhythmManager.RemoveRhythmEntity(this);
            Instantiate(explosionPrefab, invader.transform.position, Quaternion.identity);
            invader.Kill();
            Destroy(this.gameObject);
            return;
        }

        ShieldController shield = collider.GetComponent<ShieldController>();
        if(shield)
        {
            rhythmManager.RemoveRhythmEntity(this);
            Instantiate(explosionPrefab, shield.transform.position, Quaternion.identity);
            shield.Damage();
            Destroy(this.gameObject);
            return;
        }

        UFOController ufo = collider.GetComponent<UFOController>();
        if (ufo)
        {
            rhythmManager.RemoveRhythmEntity(this);
            Instantiate(explosionPrefab, ufo.transform.position, Quaternion.identity);
            ufo.Kill();
            Destroy(this.gameObject);
            return;
        }

        PlayerController player = collider.GetComponent<PlayerController>();
        if (player)
        {
            Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            player.Reset();
            rhythmManager.RemoveRhythmEntity(this);
            GameManager.instance.DecreaseLife();
            Destroy(this.gameObject);
            return;
        }
    }
}
