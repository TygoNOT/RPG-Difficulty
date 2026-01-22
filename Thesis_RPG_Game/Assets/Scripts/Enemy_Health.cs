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
        
        GameLogic.Instance.GivePlayerXP(xpReward);
        GameLogic.Instance.EnemyKilled();

        Destroy(gameObject, animationDeadTime); 
    }
}
