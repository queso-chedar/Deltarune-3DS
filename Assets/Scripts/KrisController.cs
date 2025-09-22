using UnityEngine;
using System.Linq;

public class KrisController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float runSpeed = 6f;
    public bool useDigitalMovement = true;
    public KeyCode RunKey = KeyCode.X;
    public bool isRunning = false;
    public bool forceRun = false;   // opción manual desde Inspector

    [Header("Input Settings")]
    public float stickDeadzone = 0.3f;
    public float dpadDeadzone = 0.5f;

    [Header("References")]
    public Animator animator;

    [Header("Interacción")]
    public KeyCode interactionKey = KeyCode.C;
    public LayerMask npcLayer;
    [Tooltip("Tamaño del área de interacción (ancho, alto)")]
    public Vector2 interactionBoxSize = new Vector2(2f, 2f);
    [Tooltip("Offset del área de interacción desde la posición del jugador")]
    public Vector2 interactionBoxOffset = Vector2.zero;

    [Header("Cooldown")]
    [Tooltip("Tiempo en segundos entre interacciones")]
    public float interactionCooldownTime = 1f;
    private float interactionCooldownTimer;
    private bool canInteract;

    [Header("Idle Hold")]
    [Tooltip("Tiempo mínimo para mantener animación idle tras interactuar")]
    public float idleHoldTime = 0.05f;
    private float idleHoldTimer;
    private bool isIdleHold;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isMoving;
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

        interactionCooldownTimer = 0f;
        canInteract = true;
    }

    void Update()
    {
        // Actualizar cooldown
        if (!canInteract)
        {
            interactionCooldownTimer -= Time.deltaTime;
            if (interactionCooldownTimer <= 0f)
                canInteract = true;
        }

        // Movimiento
        moveInput = GetUniversalInput();
        if (useDigitalMovement)
        {
            moveInput.x = Mathf.Abs(moveInput.x) > dpadDeadzone ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = Mathf.Abs(moveInput.y) > dpadDeadzone ? Mathf.Sign(moveInput.y) : 0;
        }
        else if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        isMoving = moveInput.magnitude > 0.1f;
        if (isMoving) lastMoveDirection = moveInput.normalized;

        // Interacción
        if (Input.GetKeyDown(interactionKey) && canInteract)
        {
            Lol nearestNPC = GetNearestNPC();
            if (nearestNPC != null)
            {
                // Forzar idle y hold
                string idleAnim = GetIdleAnimation();
                animator.Play(idleAnim);
                currentAnimation = idleAnim;
                isIdleHold = true;
                idleHoldTimer = idleHoldTime;

                // Avanzar diálogo en NPC específico
                nearestNPC.AdvanceDialog();

                canInteract = false;
                interactionCooldownTimer = interactionCooldownTime;
            }
        }

        // Mantener idle hold
        if (isIdleHold)
        {
            idleHoldTimer -= Time.deltaTime;
            if (idleHoldTimer <= 0f)
                isIdleHold = false;
        }
        else
        {
            // Animaciones normales
            targetAnimation = GetTargetAnimation();
            if (targetAnimation != currentAnimation)
            {
                animator.Play(targetAnimation);
                currentAnimation = targetAnimation;
            }
        }
        // La tecla solo afecta si no está forzado
        if (!forceRun)
        {
            if (Input.GetKey(RunKey))
                isRunning = true;
            else
                isRunning = false;
        }
        else
        {
            isRunning = true; // forzado
        }

        // Aplicar velocidad y animación
        if (isRunning)
        {
            animator.speed = 1.5f;
            moveSpeed = runSpeed;
        }
        else
        {
            animator.speed = 1f;
            moveSpeed = 4f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = isMoving ? moveInput * moveSpeed : Vector2.zero;
    }

    Vector2 GetUniversalInput()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.RightArrow)) input.x += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) input.x -= 1;
        if (Input.GetKey(KeyCode.UpArrow)) input.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) input.y -= 1;
        input.x += Input.GetAxisRaw("Horizontal");
        input.y += Input.GetAxisRaw("Vertical");
        return Vector2.ClampMagnitude(input, 1f);
    }

    string GetTargetAnimation()
    {
        if (isMoving)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                return moveInput.x > 0 ? "Walk_Right" : "Walk_Left";
            return moveInput.y > 0 ? "Walk_Up" : "Walk_Down";
        }
        return GetIdleAnimation();
    }

    public string GetIdleAnimation()
    {
        if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
            return lastMoveDirection.x > 0 ? "Idle_Right" : "Idle_Left";
        return lastMoveDirection.y > 0 ? "Idle_Up" : "Idle_Down";
    }

    Lol GetNearestNPC()
    {
        Vector2 center = (Vector2)transform.position + interactionBoxOffset;
        return Physics2D.OverlapBoxAll(center, interactionBoxSize, 0f, npcLayer)
            .Select(c => c.GetComponentInParent<Lol>())
            .Where(n => n != null)
            .OrderBy(n => Vector2.Distance(transform.position, n.transform.position))
            .FirstOrDefault();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector2 boxCenter = (Vector2)transform.position + interactionBoxOffset;
        Gizmos.DrawWireCube(boxCenter, interactionBoxSize);
    }
    public void ToggleForceRun()
{
    forceRun = !forceRun; // cambia entre true y false cada vez que se llame
}
}