using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordyAttack : MonoBehaviour
{
    [Header("Slash Settings")]
    public GameObject slashPrefab;

    [Header("Slash Point Offsets")]
    public float offsetX = 0.5f;
    public float offsetY = 0.5f;

    [Header("Timings")]
    public float attackCooldown = 1.2f;

    private bool canAttack = true;
    private Transform slashPoint;
    private Transform player;

    void Awake()
    {
        slashPoint = new GameObject("EnemySlashPoint").transform;
        slashPoint.SetParent(transform);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TryAttack()
    {
        if (!canAttack) return;

        DoSlashAttack();
        canAttack = false;

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void DoSlashAttack()
    {
        if (player == null) return;

        float horizontalDir = (player.position.x - transform.position.x) >= 0 ? 1f : -1f;

        slashPoint.localPosition = new Vector3(offsetX * horizontalDir, offsetY, 0);

        GameObject slash = Instantiate(slashPrefab, slashPoint.position, Quaternion.identity);

        slash.transform.SetParent(slashPoint);

        SlashEffect effect = slash.GetComponent<SlashEffect>();
        effect.Setup(horizontalDir);

        Destroy(slash, effect == null ? 0.2f : 0.2f);
    }
}
