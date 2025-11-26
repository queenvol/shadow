using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    private int currentHP;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 4f;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isFlashing = false;
    private Rigidbody2D rb;

    void Start()
    {
        currentHP = maxHP;
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (sr != null)
            originalColor = sr.color;
    }

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        currentHP -= damage;

        if (!isFlashing && sr != null)
            StartCoroutine(HitFlashRoutine());

        ApplyKnockback(hitDir);

        if (currentHP <= 0)
            Die();
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector2.zero);
    }

    IEnumerator HitFlashRoutine()
    {
        isFlashing = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
        isFlashing = false;
    }

    void ApplyKnockback(Vector2 dir)
    {
        if (rb == null) return;

        rb.velocity = Vector2.zero;

        if (dir != Vector2.zero)
            rb.AddForce(dir.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    void Die()
    {
        Debug.Log("Player Dead");
    }
}
