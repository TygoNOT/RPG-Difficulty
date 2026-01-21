using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;

    [Header("Level Objects")]
    public GameObject objectToDisable;   
    public GameObject objectToEnable;    

    [Header("UI")]
    public GameObject deathMenu;

    private int enemiesAlive;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Enemy_Health[] enemies = FindObjectsOfType<Enemy_Health>();
        enemiesAlive = enemies.Length;

        objectToEnable.SetActive(false);
        deathMenu.SetActive(false);
    }
    public void EnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            objectToDisable.SetActive(false);
            objectToEnable.SetActive(true);
        }
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
