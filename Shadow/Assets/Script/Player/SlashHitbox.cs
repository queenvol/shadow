using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public int damage = 1;

    private SlashEffect effect;

    void Awake()
    {
        effect = GetComponent<SlashEffect>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Shield>() != null)
            return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Transform attacker = transform.parent.parent;

            Vector2 attackDir = other.transform.position - attacker.position;

            enemy.TakeDamage(damage, attackDir);
            return;
        }

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            Transform attacker = transform.parent.parent;
            Vector2 attackDir = other.transform.position - attacker.position;
            ph.TakeDamage(damage, attackDir);
        }
    }

}
