using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public Difficulty difficulty = Difficulty.Easy;

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int damage = 1;

    private int startLevel;
    private int startXP;
    private int startXPToNext;
    private int startMaxHealth;
    private int startCurrentHealth;
    private int startDamage;
    private bool hasCheckpoint = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckpoint()
    {
        startLevel = level;
        startXP = currentXP;
        startXPToNext = xpToNextLevel;

        startMaxHealth = maxHealth;
        startCurrentHealth = currentHealth;

        startDamage = damage;

        hasCheckpoint = true;

        Debug.Log("Checkpoint saved (level start)");
    }

    public void RestoreCheckpoint()
    {
        if (!hasCheckpoint)
        {
            Debug.Log("No checkpoint -> using base stats");
            return;
        }

        level = startLevel;
        currentXP = startXP;
        xpToNextLevel = startXPToNext;

        maxHealth = startMaxHealth;
        currentHealth = startCurrentHealth;

        damage = startDamage;

        Debug.Log("Checkpoint restored");
    }

    public void SavePlayer(PlayerStats stats, Player_Lvl lvl, Player_Combat combat)
    {
        level = lvl.Level;
        currentXP = lvl.CurrentXP;
        xpToNextLevel = lvl.XPToNextLevel;

        maxHealth = stats.maxHealth;
        currentHealth = stats.currentHealth;

        damage = combat.damage;
    }

    public void LoadPlayer(PlayerStats stats, Player_Lvl lvl, Player_Combat combat)
    {
        lvl.SetData(level, currentXP, xpToNextLevel);
        stats.maxHealth = maxHealth;
        stats.currentHealth = currentHealth;
        stats.UpdateHealthUI();
        combat.damage = damage;
    }

    public void ResetToBase(int baseHP, int baseDamage, int baseXPToNext)
    {
        level = 1;
        currentXP = 0;
        xpToNextLevel = baseXPToNext;

        maxHealth = baseHP;
        currentHealth = baseHP;

        damage = baseDamage;

        hasCheckpoint = false;
    }

    public void StartNewGame(Difficulty diff, int firstLevelIndex)
    {
        difficulty = diff;

        int baseHP = 5;
        int baseDamage = 1;
        int baseXPToNext = 100;


        switch (difficulty)
        {
            case Difficulty.Easy:
                break;

            case Difficulty.Hard:
                break;

            case Difficulty.Adaptive:
                break;
        }

        ResetToBase(baseHP, baseDamage, baseXPToNext);

        SceneManager.LoadScene(firstLevelIndex);
    }

}