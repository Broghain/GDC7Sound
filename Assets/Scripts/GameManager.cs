using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private UIController UI;

    private int score;

    private bool gamePaused;
    private bool gameStarted;
    private bool gameOver;
    
    private bool countdownStarted;
    private float startTimer;

    private int livesCount = 3;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        UI = GameObject.FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();

        StartGame();
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
                audioSource.Play();
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
                StartGame();
                countdownStarted = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                Application.LoadLevel(Application.loadedLevel);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !gameStarted)
        {
            TogglePause();
        }

        if (gamePaused)
        {
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

    public void StartGame()
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
            audioSource.UnPause();
            
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 0;
            audioSource.Pause();
        }
        UI.SetPausePanel(gamePaused);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        score = Mathf.Clamp(score, 0, 999999999);
        UI.SetScoreText(score.ToString());
    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
        score = Mathf.Clamp(score, 0, 999999999);
        UI.SetScoreText(score.ToString());
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void DecreaseLife()
    {
        if (livesCount > 0)
        {
            UI.DisableLifeImg(livesCount);
            livesCount--;
            if (livesCount == 0)
            {
                UI.SetCenterText("Game Over");
                gameOver = true;
                Time.timeScale = 0;
            }
        }
    }
}
