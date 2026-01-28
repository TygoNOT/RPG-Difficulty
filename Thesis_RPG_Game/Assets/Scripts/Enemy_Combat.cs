using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy_Movment movement;

    [Header("Attribute")]
    public int damage = 1;
    public Transform attackUp;
    public Transform attackDown;
    public Transform attackLeft;
    public Transform attackRight; public float weaponRange;
    public LayerMask playerLayer;

    private void Awake()
    {
        if (movement == null)
            movement = GetComponent<Enemy_Movment>();

        float dmgMultiplier = 1f;

        if (GameSession.Instance != null)
            dmgMultiplier = GameSession.Instance.GetEnemyDamageMultiplier();

        damage = Mathf.RoundToInt(damage * dmgMultiplier);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
      /*
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().ChangeHealth(damage);
        }
      */
    }

    public void Attack()
    {
        Transform point = GetAttackPoint();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            point.position,
            weaponRange,
            playerLayer
        );

        if (hits.Length > 0)
            hits[0].GetComponent<PlayerStats>().ChangeHealth(damage);

        Debug.Log("Attack direction: " + movement.LastMoveDir);
    }



    private Transform GetAttackPoint()
    {
        Vector2 dir = movement.LastMoveDir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? attackRight : attackLeft;
        else
            return dir.y > 0 ? attackUp : attackDown;
    }
}
