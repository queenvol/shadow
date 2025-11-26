using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordy : MonoBehaviour, Enemy
{
    [Header("Stats")]
    public int maxHP = 5;
    private int currentHP;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 6f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public int attackDamage = 1;
    public float attackCooldown = 1.0f;
    private bool canAttack = true;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Color originalColor;
    private bool isFlashing = false;
    private SwordyAttack swordyAttack;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        swordyAttack = GetComponent<SwordyAttack>();
        originalColor = sr.color;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
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
            {
                swordyAttack.TryAttack();
            }
        }
    }

    void AttackPlayer()
    {
        canAttack = false;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(attackDamage);

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        currentHP -= damage;

        HitFlash();
        Knockback(hitDir);

        if (currentHP <= 0)
            Die();
    }

    void HitFlash()
    {
        if (!isFlashing)
            StartCoroutine(HitFlashRoutine());
    }

    IEnumerator HitFlashRoutine()
    {
        isFlashing = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
        isFlashing = false;
    }

    void Knockback(Vector2 dir)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(dir.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
