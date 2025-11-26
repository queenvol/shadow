using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloneMovement : MonoBehaviour
{
    public float moveSpeed = 6f;

    private PlayerInputActions input;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;

    void Awake()
    {
        input = new PlayerInputActions();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMoveCanceled;
    }

    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = -ctx.ReadValue<float>();

        if (moveInput != 0)
            sr.flipX = moveInput < 0;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}
