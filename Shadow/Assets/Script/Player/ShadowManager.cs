using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowManager : MonoBehaviour
{
    [Header("Clone Settings")]
    public GameObject clonePrefab;
    public float summonRadius = 5f;
    public LayerMask groundLayer;

    [Header("Clone Timing")]
    public float cloneLifetime = 3f;
    public float summonCooldown = 5f;

    private PlayerInputActions input;
    private Transform clone;
    private Camera cam;

    private bool isOnCooldown = false;
    private bool hasSwapped = false;

    void Awake()
    {
        input = new PlayerInputActions();
        cam = Camera.main;

        input.Player.SummonClone.performed += ctx => TrySummonClone();
    }

    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();

    void TrySummonClone()
    {
        if (isOnCooldown || clone != null)
            return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float mouseX = mouseWorld.x;

        float playerX = transform.position.x;
        float targetX = Mathf.Clamp(mouseX, playerX - summonRadius, playerX + summonRadius);

        float castHeight = transform.position.y + 5f;
        Vector2 castOrigin = new Vector2(targetX, castHeight);

        RaycastHit2D hit = Physics2D.Raycast(
            castOrigin,
            Vector2.down,
            20f,
            groundLayer
        );

        if (!hit)
            return;

        Vector2 spawnPos = new Vector2(targetX, hit.point.y);

        clone = Instantiate(clonePrefab, spawnPos, Quaternion.identity).transform;

        hasSwapped = false;

        StartCoroutine(CloneLifetimeRoutine());
    }

    IEnumerator CloneLifetimeRoutine()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(cloneLifetime);

        if (clone != null)
        {
            Destroy(clone.gameObject);
            clone = null;
        }

        yield return new WaitForSeconds(summonCooldown);


        isOnCooldown = false;
    }

    public Transform GetClone() => clone;

    public void ClearClone()
    {
        clone = null;
    }

    public bool HasSwapped() => hasSwapped;
    public void SetHasSwapped(bool value) => hasSwapped = value;
}
