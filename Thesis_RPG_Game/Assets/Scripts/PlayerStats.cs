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
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
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
