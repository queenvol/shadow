using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [Header("Right Slash Angles")]
    public float rightStartAngle = 60f;
    public float rightEndAngle = -60f;

    [Header("Left Slash Angles")]
    public float leftStartAngle = 120f;
    public float leftEndAngle = 240f;

    [Header("Movement")]
    public float rotateSpeed = 800f;

    private float currentAngle;
    private float targetAngle;

    public float AttackDir { get; private set; } = 1f;

    public void Setup(float dir)
    {
        AttackDir = Mathf.Sign(dir) == 0 ? 1f : Mathf.Sign(dir);

        if (dir >= 0f)
        {
            currentAngle = rightStartAngle;
            targetAngle = rightEndAngle;
        }
        else
        {
            currentAngle = leftStartAngle;
            targetAngle = leftEndAngle;
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void Update()
    {
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
