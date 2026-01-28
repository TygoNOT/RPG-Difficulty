using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public Difficulty difficulty = Difficulty.Easy;

    [Header("Base Player Stats (from Inspector)")]
    public int baseMaxHealth = 5;
    public int baseDamage = 1;
    public int baseXPToNext = 100;

    [Header("First Level Index")]
    public int firstLevelIndex = 1;


    [Header("Difficulty Multipliers")]

    [Header("Easy")]
    public float easyEnemyHp = 0.8f;
    public float easyEnemyDamage = 0.8f;
    public float easyXP = 1.2f;

    [Header("Hard")]
    public float hardEnemyHp = 1.4f;
    public float hardEnemyDamage = 1.3f;
    public float hardXP = 0.8f;

    [Header("Adaptive (base)")]
    public float adaptiveEnemyHp = 1.0f;
    public float adaptiveEnemyDamage = 1.0f;
    public float adaptiveXP = 1.0f;

    [Header("Adaptive Difficulty")]
    public float adaptiveFactor = 0f;
    public float adaptiveStep = 0.1f;
    public float adaptiveMin = -0.3f;
    public float adaptiveMax = 0.3f;
    
    public int deathsThisLevel = 0;
    public int hpLostThisLevel = 0;

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

        deathsThisLevel = 0;
        hpLostThisLevel = 0;
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

    public void ResetToBase()
    {
        level = 1;
        currentXP = 0;
        xpToNextLevel = baseXPToNext;

        maxHealth = baseMaxHealth;
        currentHealth = baseMaxHealth;

        damage = baseDamage;

        hasCheckpoint = false;
    }

    public void StartNewGame(Difficulty diff, int firstLevelIndex)
    {
        difficulty = diff;

        switch (difficulty)
        {
            case Difficulty.Easy:
                break;

            case Difficulty.Hard:
                break;

            case Difficulty.Adaptive:
                break;
        }

        ResetToBase();

        SceneManager.LoadScene(firstLevelIndex);
    }

    public float GetEnemyHealthMultiplier()
    {
        switch (difficulty)
        {
            case Difficulty.Easy: return easyEnemyHp;
            case Difficulty.Hard: return hardEnemyHp;
            case Difficulty.Adaptive: return adaptiveEnemyHp;
        }
        return 1.0f;
    }

    public float GetEnemyDamageMultiplier()
    {
        switch (difficulty)
        {
            case Difficulty.Easy: return easyEnemyDamage;
            case Difficulty.Hard: return hardEnemyDamage;
            case Difficulty.Adaptive: return adaptiveEnemyDamage;
        }
        return 1.0f;
    }

    public float GetXPMultiplier()
    {
        switch (difficulty)
        {
            case Difficulty.Easy: return easyXP;
            case Difficulty.Hard: return hardXP;
            case Difficulty.Adaptive: return adaptiveXP;
        }
        return 1.0f;
    }

    public void FinishLevel()
    {

        if (difficulty == Difficulty.Adaptive)
        {
            EvaluateAdaptiveDifficulty();
        }
    }
    private void EvaluateAdaptiveDifficulty()
    {
        float score = 0f;


        if (deathsThisLevel == 0) score += 0.3f;
        else if (deathsThisLevel >= 3) score -= 0.5f;
        else score -= deathsThisLevel * 0.2f;

        float hpRatio = (float)hpLostThisLevel / Mathf.Max(1, maxHealth);

        if (hpRatio < 0.3f) score += 0.3f;
        else if (hpRatio > 0.8f) score -= 0.3f;


        score = Mathf.Clamp(score, -1f, 1f);

        adaptiveFactor += score * adaptiveStep;
        adaptiveFactor = Mathf.Clamp(adaptiveFactor, adaptiveMin, adaptiveMax);

        ApplyAdaptiveMultipliers();

        Debug.Log($"ADAPTIVE SCORE: {score}, FACTOR: {adaptiveFactor}");
    }

    private void ApplyAdaptiveMultipliers()
    {
        adaptiveEnemyHp = 1.0f + adaptiveFactor;
        adaptiveEnemyDamage = 1.0f + adaptiveFactor;
        adaptiveXP = 1.0f - adaptiveFactor * 0.5f;

        adaptiveEnemyHp = Mathf.Clamp(adaptiveEnemyHp, 0.7f, 1.5f);
        adaptiveEnemyDamage = Mathf.Clamp(adaptiveEnemyDamage, 0.7f, 1.5f);
        adaptiveXP = Mathf.Clamp(adaptiveXP, 0.7f, 1.3f);
    }

    public void CheckMidLevelAdaptation()
    {
        if (difficulty != Difficulty.Adaptive)
            return;

        float score = 0f;

        if (deathsThisLevel >= 2)
            score -= 0.5f;

        float hpRatio = (float)hpLostThisLevel / Mathf.Max(1, maxHealth);

        if (hpRatio > 0.7f)
            score -= 0.4f;

        if (score < 0f)
        {
            adaptiveFactor += score * adaptiveStep;
            adaptiveFactor = Mathf.Clamp(adaptiveFactor, adaptiveMin, adaptiveMax);

            ApplyAdaptiveMultipliers();

            Debug.Log($"MID-LEVEL ADAPT -> FACTOR: {adaptiveFactor}");
        }
    }

}