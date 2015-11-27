using UnityEngine;
using System.Collections;

public class UFOController : MonoBehaviour
{
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
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, transform.position + direction, 5 * Time.deltaTime);

        if (transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
	}

    public void Kill()
    {
        GameManager.instance.IncreaseScore(1000);
        Destroy(this.gameObject);
    }
}
