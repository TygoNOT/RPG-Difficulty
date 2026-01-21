using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    Chasing,
    Attacking
}

public class Enemy_Movment : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float attackCooldownTimer;

    private bool isChasing;
    private EnemyState enemyState;
    public float attackRange = 2;
    private Vector2 moveDir;
    private Vector2 lastMoveDir = Vector2.down;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public Transform detectPoint;
    public LayerMask playerLayer;
    private bool isAttacking;
    public Vector2 LastMoveDir => lastMoveDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckForPlayer();

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        switch (enemyState)
        {
            case EnemyState.idle:
                IdleState();
                break;

            case EnemyState.Chasing:
                ChaseState();
                break;

            case EnemyState.Attacking:
                AttackState();
                break;
        }

        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir;

        UpdateAnimator();
    }

    private void IdleState()
    {
        moveDir = Vector2.zero;

        if (player != null)
        {
            enemyState = EnemyState.Chasing;
        }
    }

    private void ChaseState()
    {
        if (player == null)
        {
            enemyState = EnemyState.idle;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        moveDir = (player.position - transform.position).normalized;
    }

    private void AttackState()
    {
        moveDir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * speed;
    }
    private void UpdateAnimator()
    {
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
        animator.SetBool("IsMoving", moveDir != Vector2.zero);
        Debug.Log("IsMoving: " + (moveDir != Vector2.zero));
    }

    private void CheckForPlayer()
    {
        if (isAttacking) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                isAttacking = true;
                enemyState = EnemyState.Attacking;
                moveDir = Vector2.zero;
                animator.SetTrigger("Attack");
                return;
            }
            else if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                enemyState = EnemyState.Chasing;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            enemyState = EnemyState.idle;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = null;
            enemyState = EnemyState.idle;
            rb.velocity = Vector2.zero;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        enemyState = EnemyState.Chasing;
    }
}
