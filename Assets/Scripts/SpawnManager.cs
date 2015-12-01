using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    [SerializeField]
    private int maxRowLength = 10;

    [SerializeField]
    private GameObject invaderPrefab = null;

    [SerializeField]
    private GameObject UFOPrefab = null;
    private int spawnChance = 1;

    private int waveSize = 4;
    private int waveNum = 1;

    private Vector3 startPos;

    private List<InvaderController> invaders;

    ShieldController[] shields;

    GameManager gameManager;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        startPos = new Vector3(0, 5, 0);
        gameManager = GameManager.instance;

        invaders = new List<InvaderController>();

        shields = GameObject.FindObjectsOfType<ShieldController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManager.IsGameStarted() && invaders.Count == 0)
        {
            SpawnWave();
        }
	}

    public void SpawnWave()
    {
        foreach (ShieldController shield in shields)
        {
            shield.Reset();
        }

        int rowCount = 0;
        int row = 0;
        for (int i = 0; i < waveSize; i++)
        {
            if (rowCount == maxRowLength)
            {
                row++;
                rowCount = 0;
            }
            Vector3 startPosOffset = startPos + new Vector3(rowCount, row, 0);
            GameObject newInvader = (GameObject)Instantiate(invaderPrefab, startPosOffset, Quaternion.identity);
            InvaderController invaderController = newInvader.GetComponent<InvaderController>();

            if (maxRowLength > waveSize)
            {
                invaderController.SetRowIndex(waveSize, rowCount);
            }
            else
            {
                invaderController.SetRowIndex(maxRowLength, rowCount);
            }

            invaders.Add(invaderController);
            rowCount++;
        }
        waveSize += (waveSize / 2);
    }

    public void DecreaseEnemyCount(InvaderController invader, bool shotByPlayer)
    {
        invaders.Remove(invader);

        if (invaders.Count == 0 && shotByPlayer)
        {
            waveSize += waveNum * 2;
            gameManager.IncreaseScore(100 * waveNum);
        }

        int randomUFO = Random.Range(spawnChance, 100);
        if (randomUFO <= spawnChance)
        {
            int randomLeftOrRight = Random.Range(0, 100);
            if (randomLeftOrRight < 50)
            {
                Instantiate(UFOPrefab, new Vector3(-9, 4, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(UFOPrefab, new Vector3(9, 4, 0), Quaternion.identity);
            }
            spawnChance = 0;
        }
        spawnChance++;
    }
}
