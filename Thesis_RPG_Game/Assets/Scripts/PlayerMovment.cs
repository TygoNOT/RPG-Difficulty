using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public float playerspeed = 5;
    public int facingDirection = 1;

    [Header("References")]
    public Player_Combat player_Combat;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player_Combat.Attack();
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        rb.velocity = new Vector2(horizontal, vertical).normalized * playerspeed;
        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
