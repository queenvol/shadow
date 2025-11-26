using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAttack : MonoBehaviour
{
    public GameObject slashPrefab;

    [Header("Slash Offsets")]
    public float offsetX = 0.3f;
    public float offsetY = 0f;
    public float slashDuration = 0.2f;

    private Transform slashPoint;

    void Awake()
    {
        slashPoint = new GameObject("CloneSlashPoint").transform;
        slashPoint.SetParent(transform);
    }

    void OnEnable()
    {
        AttackEvent.OnPlayerAttacked += OnPlayerAttack;
    }

    void OnDisable()
    {
        AttackEvent.OnPlayerAttacked -= OnPlayerAttack;
    }

    void OnPlayerAttack(float playerDir)
    {
        float cloneDir = -playerDir;

        slashPoint.localPosition = new Vector3(offsetX * cloneDir, offsetY, 0);

        GameObject slash = Instantiate(slashPrefab, slashPoint.position, Quaternion.identity);
        slash.transform.SetParent(slashPoint);

        SlashEffect effect = slash.GetComponent<SlashEffect>();
        effect.Setup(cloneDir);

        Destroy(slash, slashDuration);
    }
}
