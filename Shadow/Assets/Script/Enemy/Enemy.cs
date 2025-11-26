using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    void TakeDamage(int damage, Vector2 hitDir);
}
