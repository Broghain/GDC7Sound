using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites;
    private int currentSpriteIndex = 0;

    private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Damage()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex >= sprites.Length)
        {
            gameObject.SetActive(false);
        }
        else
        {
            renderer.sprite = sprites[currentSpriteIndex];
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        currentSpriteIndex = 0;
        renderer.sprite = sprites[currentSpriteIndex];
    }
}
