using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    [SerializeField]
    private int maxRowLength = 10;

    [SerializeField]
    private GameObject invaderPrefab = null;

    private int waveSize = 4;

    private Vector3 startPos;

    private int spawnedInvaders = 0;

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        startPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height + 32, 0));
        gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManager.IsGameStarted() && spawnedInvaders == 0)
        {
            SpawnWave();
        }
	}

    public void SpawnWave()
    {
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

            if (maxRowLength > waveSize)
            {
                newInvader.GetComponent<InvaderController>().SetRowIndex(waveSize, rowCount);
            }
            else
            {
                newInvader.GetComponent<InvaderController>().SetRowIndex(maxRowLength, rowCount);
            }
                
            spawnedInvaders++;
            rowCount++;
        }
        waveSize += (waveSize / 2);
    }

    public void DecreaseEnemyCount()
    {
        spawnedInvaders--;
    }
}
