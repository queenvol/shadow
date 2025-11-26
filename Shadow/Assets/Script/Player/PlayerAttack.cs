using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public GameObject slashPrefab;

    [Header("Slash Point Offsets")]
    public float offsetX = 0.5f;
    public float offsetY = 0.5f;

    public float slashDuration = 0.2f;

    private PlayerInputActions input;
    private Camera cam;
    private Transform slashPoint;

    void Awake()
    {
        input = new PlayerInputActions();
        cam = Camera.main;

        input.Player.Attack.performed += ctx => DoAttack();

        slashPoint = new GameObject("SlashPoint").transform;
        slashPoint.SetParent(transform);
    }

    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();

    void DoAttack()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0;

        float horizontalDir = (mouseWorld.x - transform.position.x) >= 0 ? 1f : -1f;

        slashPoint.localPosition = new Vector3(offsetX * horizontalDir, offsetY, 0);

        GameObject slash = Instantiate(slashPrefab, slashPoint.position, Quaternion.identity);

        SlashEffect effect = slash.GetComponent<SlashEffect>();
        effect.Setup(horizontalDir);

        GetComponent<AttackEvent>()?.PlayerDidAttack(horizontalDir);

        Destroy(slash, slashDuration);
    }
}
