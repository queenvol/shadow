using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyDummy dummy = other.GetComponent<EnemyDummy>();
        if (dummy != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            dummy.TakeDamage(damage, dir);
        }
    }
}
