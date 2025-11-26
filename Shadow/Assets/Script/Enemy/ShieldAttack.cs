using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth p = other.GetComponent<PlayerHealth>();
        if (p != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            p.TakeDamage(damage, dir);
        }
    }
}
