using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 8;
    private int currentHP;

    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float chaseRange = 5f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public float shieldPushForce = 10f;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 3f;

    [Header("References")]
    public BoxCollider2D shieldBlock;
    public BoxCollider2D shieldAttackCollider;
    public SpriteRenderer sr;

    private Transform player;
    private Rigidbody2D rb;
    private Color originalColor;

    private bool facingRight = true;
    private bool isFlashing = false;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        originalColor = sr.color;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        shieldAttackCollider.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        FlipToPlayer();

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

            if (canAttack)
                StartCoroutine(ShieldPush());
        }
    }

    void FlipToPlayer()
    {
        if (player.position.x < transform.position.x && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.position.x > transform.position.x && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    bool IsFrontBlocked(Vector2 attackDir)
    {
        float facing = Mathf.Sign(transform.right.x);
        float attackSide = Mathf.Sign(attackDir.x);

        return facing == attackSide;
    }

    public void TakeDamage(int damage, Vector2 attackDir)
    {
        currentHP -= damage;

        HitFlash();
        Knockback(attackDir);

        if (currentHP <= 0)
            Die();
    }

    void ShieldBlockFeedback()
    {
        if (!isFlashing)
            StartCoroutine(BlockFlashRoutine());
    }

    IEnumerator BlockFlashRoutine()
    {
        isFlashing = true;
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.10f);
        sr.color = originalColor;
        isFlashing = false;
    }

    IEnumerator ShieldPush()
    {
        canAttack = false;

        Vector3 originalPos = transform.localPosition;
        float dir = transform.localScale.x;

        float backDist = 0.15f;
        Vector3 backTarget = originalPos + new Vector3(-dir * backDist, 0, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10f;
            transform.localPosition = Vector3.Lerp(originalPos, backTarget, t);
            yield return null;
        }

        shieldAttackCollider.enabled = true;

        float forwardDist = 0.35f;
        Vector3 forwardTarget = originalPos + new Vector3(dir * forwardDist, 0, 0);

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 12f;
            transform.localPosition = Vector3.Lerp(backTarget, forwardTarget, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        shieldAttackCollider.enabled = false;

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10f;
            transform.localPosition = Vector3.Lerp(forwardTarget, originalPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
