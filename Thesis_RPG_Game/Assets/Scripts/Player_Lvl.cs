using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Lvl : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;

    [Header("Growth Per Level")]
    [SerializeField] private int maxHpPerLevel = 10;
    [SerializeField] private int damagePerLevel = 2;
    [SerializeField] private int xpIncreasePerLevel = 50;

    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Player_Combat playerCombat;

    public event System.Action<int, int, int> OnXPChanged;
    public event System.Action<int> OnLevelUp;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;
    public int Level => level;

    private void Start()
    {
        OnXPChanged?.Invoke(currentXP, xpToNextLevel, level);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel, level);
    }
    private void LevelUp()
    {
        level++;

        xpToNextLevel += xpIncreasePerLevel;

        playerStats.IncreaseMaxHealth(maxHpPerLevel);
        playerCombat.IncreaseDamage(damagePerLevel);

        Debug.Log($"LEVEL UP! New level: {level}");

        OnLevelUp?.Invoke(level);
    }

    public void SetData(int newLevel, int newXP, int newXPToNext)
    {
        level = newLevel;
        currentXP = newXP;
        xpToNextLevel = newXPToNext;

        OnXPChanged?.Invoke(currentXP, xpToNextLevel, level);
    }

}
