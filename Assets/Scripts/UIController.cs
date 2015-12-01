using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;


    [SerializeField]
    private Text centerText, scoreText, instructionText;

    [SerializeField]
    private GameObject[] livesImages;

    public void SetCenterText(string text)
    {
        centerText.text = text;
    }

    public void SetScoreText(string text)
    {
        scoreText.text = "Score: " + text;
    }

    public void SetPausePanel(bool state)
    {
        pausePanel.SetActive(state);
    }

    public void SetInstructionText(string text)
    {
        instructionText.text = text;
    }

    public void DisableLifeImg(int lives)
    {
        livesImages[lives - 1].SetActive(false);
    }

    public void EnableLifeImg(int lives)
    {
        livesImages[lives].SetActive(true);
    }
}
