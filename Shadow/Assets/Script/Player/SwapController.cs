using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapController : MonoBehaviour
{
    private PlayerInputActions input;
    private ShadowManager shadowManager;

    void Awake()
    {
        input = new PlayerInputActions();

        shadowManager = GetComponent<ShadowManager>();

        input.Player.Swap.performed += ctx => TrySwap();
    }

    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();

    void TrySwap()
    {
        Transform clone = shadowManager.GetClone();
        if (clone == null) return;

        if (shadowManager.HasSwapped()) return;

        Vector3 p = transform.position;
        transform.position = clone.position;
        clone.position = p;

        shadowManager.SetHasSwapped(true);
    }
}
