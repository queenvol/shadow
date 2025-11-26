using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Shield>() != null)
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 attackDir = other.transform.position - transform.parent.position;
            enemy.TakeDamage(damage, attackDir);
            return;
        }

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }

}
