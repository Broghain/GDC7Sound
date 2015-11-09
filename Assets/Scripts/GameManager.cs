using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private UIController UI;

    private int score;

    private bool gamePaused;
    private bool gameStarted;
    
    private bool countdownStarted;
    private float startTimer;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        UI = GameObject.FindObjectOfType<UIController>();
        StartGame(false);
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();

        if (countdownStarted && startTimer > -1.0f)
        {
            UI.SetInstructionText("P = Pause");
            startTimer -= Time.deltaTime;
            if (startTimer < 0.0f)
            {
                UI.SetCenterText("Start!");
            }
            else
            {
                UI.SetCenterText(((int)startTimer+1).ToString());
            }
        }
        else if (countdownStarted)
        {
            countdownStarted = false;
            gameStarted = true;
            UI.SetCenterText("");
        }
	}

    void GetInput()
    {
        if (!gameStarted && !countdownStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartGame(false);
                countdownStarted = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                TogglePause();
                StartGame(true);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                TogglePause();
                Application.LoadLevel(Application.loadedLevel);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }       
    }

    public void StartGame(bool keepSong)
    {
        score = 0;
        UI.SetScoreText(score.ToString());
        UI.SetInstructionText("ESC = Exit");
        UI.SetCenterText("Enter = Start Game");
        startTimer = 3.0f;
    }

    public void TogglePause()
    {
        if(Time.timeScale == 0)
        {
            gamePaused = false;
            Time.timeScale = 1;
            
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 0;
        }
        UI.SetPausePanel(gamePaused);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UI.SetScoreText(score.ToString());
    }
}
