using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public static Action<float> OnPlayerAttacked;

    public void PlayerDidAttack(float dir)
    {
        OnPlayerAttacked?.Invoke(dir);
    }
}
