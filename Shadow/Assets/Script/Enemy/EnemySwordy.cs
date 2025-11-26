using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordy : EnemyBase
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 6f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public int attackDamage = 1;
    public float attackCooldown = 1.0f;

    private Transform player;
    private SwordyAttack swordyAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        swordyAttack = GetComponent<SwordyAttack>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > chaseRange)
        {
            rb.velocity = Vector2.zero;
        }
        else if (distance > attackRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (swordyAttack != null)
                swordyAttack.TryAttack();
        }
    }
}
