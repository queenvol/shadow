using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, dir);
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            player.TakeDamage(damage, dir);
        }
    }
}
