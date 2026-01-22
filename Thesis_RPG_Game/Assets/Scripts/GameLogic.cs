using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;

    [Header("Level Objects")]
    public GameObject objectToDisable;   
    public GameObject objectToEnable;

    [Header("UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    public GameObject deathMenu;
    private Player_Lvl playerLevel;
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
        playerLevel = FindObjectOfType<Player_Lvl>();
        playerLevel.OnXPChanged += UpdateXPUI;
        UpdateXPUI(playerLevel.CurrentXP, playerLevel.XPToNextLevel, playerLevel.Level);

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

        if (GameSession.Instance != null)
        {
            GameSession.Instance.RestoreCheckpoint();
        }

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

    public void GivePlayerXP(int amount)
    {
        playerLevel.AddXP(amount);
    }

    private void UpdateXPUI(int xp, int xpToNext, int lvl)
    {
        xpSlider.maxValue = xpToNext;
        xpSlider.value = xp;
        levelText.text = "LVL " + lvl;
        xpText.text = $"{xp} / {xpToNext}";
    }
}
