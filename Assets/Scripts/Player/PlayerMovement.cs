using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 12f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps = 1;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX = 6f;
    [SerializeField] private float wallJumpY = 12f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    [Header("Mobile Controls")]
    [SerializeField] private Joystick joystick;   // drag MovementJoystick here in Inspector

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // ✅ Fix for first jump issue
        coyoteCounter = coyoteTime;
        jumpCounter = extraJumps;
    }

    private void Update()
    {
        // --- INPUT ---
#if UNITY_STANDALONE || UNITY_WEBGL
        horizontalInput = Input.GetAxis("Horizontal");   // PC keyboard
#elif UNITY_ANDROID || UNITY_IOS
        horizontalInput = joystick != null ? joystick.Horizontal : 0f;   // Mobile joystick
#endif

        // --- FLIP SPRITE ---
        if (horizontalInput > 0.01f)
            spriteRenderer.flipX = false;
        else if (horizontalInput < -0.01f)
            spriteRenderer.flipX = true;

        // --- ANIMATOR ---
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());

        // --- JUMP KEYBOARD ---
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // --- VARIABLE JUMP HEIGHT ---
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);

        // --- PHYSICS ---
        if (OnWall())
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (IsGrounded())
            {
                // ✅ Reset jump & coyote when grounded
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }

    // --- PUBLIC JUMP (for Button) ---
    public void Jump()
    {
        if (coyoteCounter <= 0 && !OnWall() && jumpCounter <= 0) return;

        if (jumpSound != null)
            SoundManager.instance.PlaySound(jumpSound);

        if (OnWall())
        {
            WallJump();
        }
        else
        {
            if (IsGrounded() || coyoteCounter > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
            else if (jumpCounter > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                jumpCounter--;
            }

            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY);
        wallJumpCooldown = 0;
    }

    // --- CHECKS ---
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.2f,
            groundLayer
        );

        // Debug draw in Scene view
        Color rayColor = raycastHit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * 0.2f, rayColor);

        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        return raycastHit.collider != null;
    }

    // 🔽 back to lowercase so PlayerAttack.cs works
    public bool canAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}
