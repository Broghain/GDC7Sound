﻿using UnityEngine;
using System.Collections;

public class InvaderController : RhythmBehaviour
{
    private SpawnManager spawnManager;
    private RhythmManager rhythmManager;
    
    private Vector3 nextPosition;

    private int rowLength; //Length of invader row I am in
    private int rowIndex; //What position in row do I have

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject projecilePrefab;

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
        nextPosition = transform.position;
        spawnManager = SpawnManager.instance;
        rhythmManager = RhythmManager.instance;
        rhythmManager.AddRhythmEntity(this);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, nextPosition, 10 * Time.deltaTime);

        if (transform.position.y <= -2.5f)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameManager.instance.DecreaseScore(500);
            spawnManager.DecreaseEnemyCount(this);
            rhythmManager.RemoveRhythmEntity(this);
            Destroy(this.gameObject);
        }
	}

    void MoveToNextPos()
    {
        if (nextStep == Step.Right && nextPosition.x + (rowLength - rowIndex) > maxX)
        {
            nextStep = Step.Down;
            previousStep = Step.Right;
        }
        else if (nextStep == Step.Left && nextPosition.x - (1 + rowIndex) < minX)
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
                nextPosition += Vector3.right;
                break;
            case Step.Left:
                nextPosition += Vector3.left;
                break;
            case Step.Down:
                nextPosition += Vector3.down;
                break;
        }
    }

    void ShootProjectile()
    {
        int random = Random.Range(0, 100);
        if (random < 2)
        {
            Instantiate(projecilePrefab, nextPosition + Vector3.down, Quaternion.identity);
        }
    }

    public override void RhythmicUpdate()
    {
        MoveToNextPos();
        ShootProjectile();
    }

    public void SetRowIndex(int rowLength, int rowIndex)
    {
        this.rowLength = rowLength;
        this.rowIndex = rowIndex;
    }

    public void Kill()
    {
        GameManager.instance.IncreaseScore(50);
        spawnManager.DecreaseEnemyCount(this);
        rhythmManager.RemoveRhythmEntity(this);
        Destroy(this.gameObject);
    }
}
