using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    public float timer = 0.5f;

    private Vector3 startPosition;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.IsGameStarted())
        {
            Vector3 nextPosition = transform.position;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                nextPosition = transform.position + Vector3.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                nextPosition = transform.position + Vector3.right;
            }
            nextPosition.x = Mathf.Clamp(nextPosition.x, -7, 7);
            transform.position = Vector3.Lerp(transform.position, nextPosition, 10 * Time.deltaTime);


            timer -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Z) && timer <= 0)
            {
                Instantiate(bulletPrefab, transform.position + (Vector3.up/2), Quaternion.identity);
                timer = 0.5f;
            }
        }
	}

    public void Reset()
    {
        transform.position = startPosition;
    }
}
