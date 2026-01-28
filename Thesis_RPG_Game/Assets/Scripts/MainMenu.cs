using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject difficultyPanel;

    [Header("First Level Index")]
    public int firstLevelIndex = 1; 
    public int mainMenuIndex = 0;
    private void Start()
    {
        mainPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    public void OpenDifficultyMenu()
    {
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void BackToMain()
    {
        difficultyPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void StartEasy()
    {
        GameSession.Instance.StartNewGame(Difficulty.Easy, firstLevelIndex);
    }

    public void StartHard()
    {
        GameSession.Instance.StartNewGame(Difficulty.Hard, firstLevelIndex);
    }
    public void StartAdaptive()
    {
        GameSession.Instance.StartNewGame(Difficulty.Adaptive, firstLevelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

}

