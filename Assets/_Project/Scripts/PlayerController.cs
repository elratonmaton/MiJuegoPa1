using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 7f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 16f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer = ~0;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Debug.Log($"[PlayerController] Awake en {name}. rb={rb != null} sr={sr != null}");
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
        {
            Debug.LogWarning("[PlayerController] Keyboard.current es null");
            return;
        }

        horizontalInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) horizontalInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontalInput += 1f;

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position, groundCheckRadius, groundLayer) != null;
        }

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (horizontalInput != 0f && sr != null)
        {
            sr.flipX = horizontalInput < 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}