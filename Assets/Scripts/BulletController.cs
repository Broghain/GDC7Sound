using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
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
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, nextPosition, 10 * Time.deltaTime);
        nextPosition = transform.position + moveDirection.normalized;
        if (transform.position.y > topRight.y || transform.position.y < bottomLeft.y)
        {
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        InvaderController invader = collider.GetComponent<InvaderController>();
        if (invader)
        {
            Instantiate(explosionPrefab, invader.transform.position, Quaternion.identity);
            invader.Kill();
            Destroy(this.gameObject);
            return;
        }

        ShieldController shield = collider.GetComponent<ShieldController>();
        if(shield)
        {
            Instantiate(explosionPrefab, shield.transform.position, Quaternion.identity);
            shield.Damage();
            Destroy(this.gameObject);
            return;
        }

        UFOController ufo = collider.GetComponent<UFOController>();
        if (ufo)
        {
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
            GameManager.instance.DecreaseLife();
            Destroy(this.gameObject);
            return;
        }
    }
}
