using UnityEngine;

public class KrisController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public bool useDigitalMovement = true;

    [Header("Input Settings")]
    public float stickDeadzone = 0.3f;
    public float dpadDeadzone = 0.5f;

    [Header("References")]
    public Animator animator;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isMoving;

    // Variables para seguimiento de animaciones
    private string currentAnimation;
    private string targetAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.drag = 50;
        lastMoveDirection = Vector2.down;
        currentAnimation = "Idle_Down";
        targetAnimation = "Idle_Down";
    }

    void Update()
    {
        // Capturar input con soporte universal
        moveInput = GetUniversalInput();
        
        // Manejar movimiento digital
        if (useDigitalMovement)
        {
            moveInput = new Vector2(
                Mathf.Abs(moveInput.x) > dpadDeadzone ? Mathf.Sign(moveInput.x) : 0,
                Mathf.Abs(moveInput.y) > dpadDeadzone ? Mathf.Sign(moveInput.y) : 0
            );
        }
        else
        {
            if (moveInput.magnitude > 1) moveInput.Normalize();
        }

        // Actualizar estado de movimiento
        isMoving = moveInput.magnitude > 0.1f;
        if (isMoving) lastMoveDirection = moveInput.normalized;

        // Determinar animación objetivo
        targetAnimation = GetTargetAnimation();
        
        // Cambiar animación solo si es necesario
        if (targetAnimation != currentAnimation)
        {
            animator.Play(targetAnimation);
            currentAnimation = targetAnimation;
        }
    }

    void FixedUpdate()
    {
        // Movimiento con frenado instantáneo
        rb.velocity = isMoving ? moveInput * moveSpeed : Vector2.zero;
    }

    Vector2 GetUniversalInput()
    {
        Vector2 input = Vector2.zero;
        
        // Teclado (flechas y WASD)
        if (Input.GetKey(KeyCode.RightArrow)) input.x += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) input.x -= 1;   
        if (Input.GetKey(KeyCode.UpArrow)) input.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) input.y -= 1;
        
        // Gamepads y 3DS Circle Pad
        input.x += Input.GetAxisRaw("Horizontal");
        input.y += Input.GetAxisRaw("Vertical");
        
        // Limitar valores
        input.x = Mathf.Clamp(input.x, -1f, 1f);
        input.y = Mathf.Clamp(input.y, -1f, 1f);
        
        return input;
    }

    string GetTargetAnimation()
    {
        if (isMoving)
        {
            // Determinar dirección dominante
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                return moveInput.x > 0 ? "Walk_Right" : "Walk_Left";
            }
            else
            {
                return moveInput.y > 0 ? "Walk_Up" : "Walk_Down";
            }
        }
        else
        {
            // Animación idle basada en la última dirección
            if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
            {
                return lastMoveDirection.x > 0 ? "Idle_Right" : "Idle_Left";
            }
            else
            {
                return lastMoveDirection.y > 0 ? "Idle_Up" : "Idle_Down";
            }
        }
    }
}