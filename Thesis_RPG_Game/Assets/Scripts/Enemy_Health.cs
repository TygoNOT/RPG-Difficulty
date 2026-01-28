using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("Attribute")]
    public int currentHealth;
    public int maxHealth;
    public int xpReward = 25;
    public float animationDeadTime;
    private bool isDead;
    private Animator anim;
    private Collider2D col;
    
    private void Start()
    {
        float hpMultiplier = 1f;

        if (GameSession.Instance != null)
            hpMultiplier = GameSession.Instance.GetEnemyHealthMultiplier();

        maxHealth = Mathf.RoundToInt(maxHealth * hpMultiplier);
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GetComponent<Enemy_Movment>().enabled = false;
        GetComponent<Enemy_Combat>().enabled = false;
        anim.SetBool("isDead", true);
        col.enabled = false;

        int finalXP = xpReward;

        if (GameSession.Instance != null)
        {
            float xpMultiplier = GameSession.Instance.GetXPMultiplier();
            finalXP = Mathf.RoundToInt(xpReward * xpMultiplier);
        }

        GameLogic.Instance.GivePlayerXP(finalXP);
        GameLogic.Instance.EnemyKilled();

        Destroy(gameObject, animationDeadTime); 
    }
}
