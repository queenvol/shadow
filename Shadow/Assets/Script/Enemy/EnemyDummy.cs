using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 5;
    private int currentHP;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 4f;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        currentHP = maxHP;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHP -= damage;

        HitFlash();
        Knockback(hitDirection);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(dir.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
