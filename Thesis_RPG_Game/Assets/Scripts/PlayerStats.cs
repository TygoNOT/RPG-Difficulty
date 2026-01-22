using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Attribute")]
    public int currentHealth;
    public int maxHealth;
    public TMP_Text healthText;
    private bool isDead;
    private Animator anim;
    private Rigidbody2D rb;

    public void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (GameSession.Instance != null)
        {
            GameSession.Instance.LoadPlayer(
                this,
                GetComponent<Player_Lvl>(),
                GetComponent<Player_Combat>()
            );
        }

        UpdateHealthUI();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
    }


    private void Die()
    {
        isDead = true;

        anim.SetBool("isDead", true);
        rb.velocity = Vector2.zero;

        GetComponent<PlayerMovment>().enabled = false;
        GetComponent<Player_Combat>().enabled = false;

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1.1f);

        GameLogic.Instance.PlayerDied();
    }

}
