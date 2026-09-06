using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Variables for jump mechanics
    public float maxflow = 30f;
    public float jumpforce = 12f;
    private int jumpCount = 1;
    private int maxJumpCount = 3;
    private float jumpResetCoolDown = 5f;
    private float jumpBuffer = 0f;
    private bool isGrounded = false;
    public LayerMask ground;
    public float flow = 0.0f;
    // Variables for interacting with walls
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    public bool canWallJump = true;
    private float jumpTime = 0f;

    // Variables for dashing
    private bool canDash = true;
    private float dashForce = 80f;
    //private float dashCooldownTime = 5f;
     private float dashDuration = 0.3f;
    private int maxDashCount = 1;
    private int dashCount = 1;
    private float dashCooldown = 5f;
    public float isDashing = 0f;
    public bool wasDashing = false;
    private float extraInv = 0.4f; // Extra invincibility time after dash, can be modified by power-ups
    float originalGravity = 1f;
    public delegate void playerDash(float invincibilityDuration);
    public static event playerDash playerDashed;

    public float moveSpeed = 5f;
    public float maxMoveSpeed = 5f;
    float horizontal;
    float vertical;
    public Rigidbody2D rb;
    //public InputActionReference moveAction;
    private Vector2 moveInput;

    // Knockback state
    private bool isKnockedBack = false;
    private float knockbackTimeRemaining = 0f;
    private Vector2 knockbackVelocity = Vector2.zero;

    private Animator animator;

    float timeSinceInput = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing > 0) //maybe remove
        {
            return; // Skip movement logic while dashing
        }
        else
        {
            rb.gravityScale = originalGravity; // Restore gravity when not dashing
        }
        // Sprite flipping — purely visual, not physics
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        }

        // Animation state — should update every rendered frame for smoothness
        SetAnimation(horizontal);
        // end of Update
        //Debug.Log($"[Update] vX={rb.linearVelocityX}, pos={transform.position}");
    }

    public void ApplyKnockback(Vector2 direction, float strength, float duration)
    {
        if (direction == Vector2.zero)
            return;

        isKnockedBack = true;
        knockbackTimeRemaining = duration;
        knockbackVelocity = direction.normalized * strength;

        rb.linearVelocity = new Vector2(knockbackVelocity.x, knockbackVelocity.y);
    }
    void FixedUpdate()
    {
        if(horizontal == 0)
        {
            timeSinceInput += Time.fixedDeltaTime;
            if(timeSinceInput > 0.5f)
            {
                flow = flow/2f;
                
            }
            
        }
        else
        {
            timeSinceInput = 0f;
            if(flow > maxflow)
            {
                flow = flow * 0.8f + maxflow * 0.2f;
            }
            else
            {
                flow += 3f * Time.fixedDeltaTime;
            }
        }
        if (isDashing <= 0)
        {
            if(wasDashing)
            {
                rb.gravityScale = originalGravity; // Restore gravity after dash
                if(Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
                {
                    rb.linearVelocityX = maxMoveSpeed * Mathf.Sign(rb.linearVelocityX); // Optional: Cap horizontal speed after dash
                    Debug.Log($"Capping horizontal speed after dash: {rb.linearVelocityX}");
                }
                if(Mathf.Abs(rb.linearVelocityY) > maxMoveSpeed)
                {
                    rb.linearVelocityY = maxMoveSpeed * Mathf.Sign(rb.linearVelocityY); // Optional: Cap vertical speed after dash
                    Debug.Log($"Capping vertical speed after dash: {rb.linearVelocityY}");
                }
                wasDashing = false;
            }
            
            
            if (isKnockedBack)
            {
                knockbackTimeRemaining -= Time.fixedDeltaTime;
                if (knockbackTimeRemaining <= 0f)
                {
                    isKnockedBack = false;
                    knockbackVelocity = Vector2.zero;
                }
                else
                {
                    rb.linearVelocity = new Vector2(knockbackVelocity.x, rb.linearVelocity.y);
                    return;
                }
            }
            if (horizontal != 0 && Mathf.Abs(rb.linearVelocityX) <= maxMoveSpeed) //flow*0.5f
            {
                rb.linearVelocityX = horizontal * (moveSpeed); //flow*0.5f Add a small acceleration factor based on how long the player has been moving
            }
            else if (horizontal != 0)
            {
                if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(horizontal))
                {
                    rb.linearVelocityX += horizontal;
                }
                else
                {
                    rb.linearVelocityX -= 10f * Mathf.Sign(rb.linearVelocityX) * Time.fixedDeltaTime;
                }
            }
            // If you are NOT pressing any direction
            else
            {
                // If you were just dashing (velocity is higher than max speed), gradually slow down
                if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed) //flow*0.5f
                {
                    rb.linearVelocityX -= 10f * Mathf.Sign(rb.linearVelocityX) * Time.fixedDeltaTime;
                }
                // Otherwise, instantly stop moving
                else
                {
                    rb.linearVelocityX = 0f;
                }
            }
        }
        else
        {
            wasDashing = true;
        }

        


        if (Physics2D.OverlapArea(new Vector2(rb.transform.position.x - 0.3f, rb.transform.position.y - 1.05f), new Vector2(rb.transform.position.x + 0.3f, rb.transform.position.y - 0.9f), ground) && jumpResetCoolDown > 0.2)
        {
            jumpCount = maxJumpCount;
            dashCount = maxDashCount;
            isGrounded = true;
            jumpBuffer = 0f;
        }
        else if (isGrounded)
        {
            jumpBuffer += Time.deltaTime;
            if (jumpBuffer > 0.2f)
            {
                jumpCount -= 1;
                isGrounded = false;
            }
        }

        
        //Debug.Log("horizontal: " + horizontal);
        if (Physics2D.OverlapArea(new Vector2(rb.transform.position.x-0.5f,rb.transform.position.y+0.35f),new Vector2(rb.transform.position.x-0.4f,rb.transform.position.y-0.35f), ground))
        {
            if(horizontal < 0)
            {
                if (isTouchingLeftWall == false && jumpCount != maxJumpCount && canWallJump ) //removed && jumpTime >0.5f
                {
                    jumpCount = maxJumpCount;
                    dashCount = maxDashCount;
                    
                }
                isTouchingLeftWall = true;

            }
            else
            {
                isTouchingLeftWall = false; //not sure if this is needed, but it should be fine
            }
        }
        else
        {
            isTouchingLeftWall = false;
        }
        if (isTouchingLeftWall && isGrounded == false && rb.linearVelocityY <= 0)
        {
            rb.linearVelocityY = -0.1f;
            if (rb.linearVelocityX < 0)
            {
                rb.linearVelocityX = 0f;
            }

        }

        if (Physics2D.OverlapArea(new Vector2(rb.transform.position.x + 0.4f, rb.transform.position.y + 0.35f), new Vector2(rb.transform.position.x + 0.5f, rb.transform.position.y - 0.35f), ground))
        {
            if (horizontal > 0) //removed && jumpTime>0.5f
            {
                if(isTouchingRightWall == false && jumpCount != maxJumpCount && canWallJump)
                {
                    jumpCount = maxJumpCount;
                    dashCount = maxDashCount;
                }
                isTouchingRightWall = true;
                //Debug.Log("Touching right wall");
            }
            else
            {
                isTouchingRightWall = false; //not sure if this is needed, but it should be fine
            }
            
        }
        else
        {
            isTouchingRightWall = false;
        }
        if (isTouchingRightWall && isGrounded == false && rb.linearVelocityY <= 0)
        {
            rb.linearVelocityY = -0.1f;
            if (rb.linearVelocityX > 0)
            {
                rb.linearVelocityX = 0f;
            }

        }
        
        if (isGrounded || isTouchingRightWall || isTouchingLeftWall) {
            jumpTime = 0f; }
        else
        {
            jumpTime += Time.fixedDeltaTime;
        }
        jumpResetCoolDown += Time.deltaTime;
        dashCooldown += Time.deltaTime;
        isDashing -= Time.deltaTime;
        //Debug.Log($"Flow: {flow}");
        // end of FixedUpdate, after all velocity-setting code
        //Debug.Log($"[FixedUpdate] vX={rb.linearVelocityX}, pos={transform.position}");
        //Debug.Log($"L={isTouchingLeftWall} R={isTouchingRightWall} grounded={isGrounded} velX={rb.linearVelocityX}");
    }
    void OnDrawGizmos()
    {
        // 1. Ground Check (Green)
        Gizmos.color = Color.green;
        Vector2 groundA = new Vector2(transform.position.x - 0.3f, transform.position.y - 1.05f);
        Vector2 groundB = new Vector2(transform.position.x + 0.3f, transform.position.y - 0.9f);
        DrawOverlapBox(groundA, groundB);

        // 2. Left Wall Check (Blue)
        Gizmos.color = Color.blue;
        Vector2 leftWallA = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.35f);
        Vector2 leftWallB = new Vector2(transform.position.x - 0.4f, transform.position.y - 0.35f);
        DrawOverlapBox(leftWallA, leftWallB);

        // 3. Right Wall Check (Red)
        Gizmos.color = Color.red;
        Vector2 rightWallA = new Vector2(transform.position.x + 0.4f, transform.position.y + 0.35f);
        Vector2 rightWallB = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.35f);
        DrawOverlapBox(rightWallA, rightWallB);
    }

    // Helper method to keep the code clean
    void DrawOverlapBox(Vector2 pointA, Vector2 pointB)
    {
        Vector2 center = (pointA + pointB) / 2f;
        Vector2 size = new Vector2(
            Mathf.Abs(pointA.x - pointB.x),
            Mathf.Abs(pointA.y - pointB.y)
        );
        Gizmos.DrawWireCube(center, size);
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        horizontal = moveInput.x;
        vertical = moveInput.y;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isDashing<=0)
        {
            if (jumpCount > 0 || ((isTouchingLeftWall || isTouchingRightWall) && canWallJump)) 
            {
                rb.linearVelocityY = jumpforce;
                flow += 0.5f;
                timeSinceInput = 0f;
                if (canWallJump == false || (isTouchingLeftWall == false && isTouchingRightWall == false))
                {
                    jumpCount -= 1;
                }
                isGrounded = false;
                jumpResetCoolDown = 0;
                if (isTouchingLeftWall && canWallJump && isTouchingRightWall == false)
                {
                    if(horizontal < 0)
                    {
                        rb.linearVelocityX = 10f; //flow*0.5f
                    }
                    else
                    {
                        rb.linearVelocityX = 0f;
                    }
                    
                    isTouchingLeftWall = false;
                    jumpCount -= 1;
                }
                if (isTouchingRightWall && canWallJump && isTouchingLeftWall == false)
                {
                    if(horizontal > 0)
                    {
                        rb.linearVelocityX = -10f; //-flow*0.5f
                    }
                    else
                    {
                        rb.linearVelocityX = 0f;
                    }
                    
                    isTouchingRightWall = false;
                    jumpCount -= 1;
                }
                jumpTime = 0;
            }
            
        }
        if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash && dashCount > 0 && !isGrounded) //dashCooldown >= dashCooldownTime && 
        {
            isDashing = dashDuration;
            dashCount -= 1;
            flow += 3f;
            rb.gravityScale = 0f; // Disable gravity during dash
            rb.linearVelocityY = 0f; // Optional: Reset vertical velocity to prevent upward
            wasDashing = true;
            if (vertical == 0)
            {
                rb.linearVelocityX = transform.localScale.x*dashForce;
            }
            else
            {
                rb.linearVelocityX = transform.localScale.x * dashForce /Mathf.Sqrt(2);
                rb.linearVelocityY = Mathf.Sign(vertical) * transform.localScale.y * dashForce / Mathf.Sqrt(2);
            }

            //dashCooldown = 0f;
            playerDashed.Invoke(dashDuration + extraInv);
            
            
        }
        
    }
    

    private void SetAnimation(float horizontal)
    {
        
        if (isGrounded)
        {
            if(Mathf.Abs(horizontal) > 0)
            {
                animator.Play("Run");
            }
            else
            {
                animator.Play("Idle");
            }
        }
        else
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Jump");
            }
            else
            {
                animator.Play("Fall");
            }
        }
        
    }
}
