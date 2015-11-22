using UnityEngine;
using System.Collections;

public class InvaderController : MonoBehaviour {

    private Vector3 nextPosition;

    private int rowLength;
    private int rowIndex;

    private enum Step
    {
        Left,
        Right,
        Down
    }
    private Step nextStep = Step.Right;
    private Step previousStep = Step.Down;

    private float maxX;
    private float minX;

    float timer = 0.5f;

	// Use this for initialization
	void Start () {
        minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            MoveToNextPos();
            timer = 0.5f;
        }
	}

    void MoveToNextPos()
    {
        if (nextStep == Step.Right && transform.position.x + (1 + (rowLength - rowIndex)) > maxX)
        {
            nextStep = Step.Down;
            previousStep = Step.Right;
        }
        else if (nextStep == Step.Left && transform.position.x - (1 + (rowLength + rowIndex)) < minX)
        {
            nextStep = Step.Down;
            previousStep = Step.Left;
        }
        else if (nextStep == Step.Down)
        {
            if (previousStep == Step.Right)
            {
                nextStep = Step.Left;
            }
            else if(previousStep == Step.Left)
            {
                nextStep = Step.Right;
            }
            previousStep = Step.Down;
        }

        switch (nextStep)
        {
            case Step.Right:
                transform.position = transform.position + transform.right;
                break;
            case Step.Left:
                transform.position = transform.position + -transform.right;
                break;
            case Step.Down:
                transform.position = transform.position + -transform.up;
                break;
        }
    }

    public void SetRowIndex(int rowLength, int rowIndex)
    {
        this.rowLength = rowLength;
        this.rowIndex = rowIndex;
    }
}
