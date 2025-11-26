using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, Enemy
{
    [Header("Stats")]
    public int maxHP = 5;
    protected int currentHP;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 3f;

    protected SpriteRenderer sr;
    protected Color originalColor;
    protected bool isFlashing = false;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponentInChildren<SpriteRenderer>() ?? GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    public virtual void TakeDamage(int damage, Vector2 hitDir)
    {
        currentHP -= damage;

        if (sr != null && !isFlashing)
            StartCoroutine(HitFlashRoutine());

        ApplyKnockback(hitDir);

        if (currentHP <= 0)
            Die();
    }

    protected virtual void ApplyKnockback(Vector2 dir)
    {
        if (rb == null) return;

        rb.velocity = Vector2.zero;

        if (dir != Vector2.zero)
            rb.AddForce(dir.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    protected virtual IEnumerator HitFlashRoutine()
    {
        isFlashing = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
        isFlashing = false;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
