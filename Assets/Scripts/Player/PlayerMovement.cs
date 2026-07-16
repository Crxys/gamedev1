using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Variables for jump mechanics
    public float jumpforce = 10f;
    private int jumpCount = 1;
    private int maxJumpCount = 3;
    private float jumpResetCoolDown = 5f;
    private float jumpBuffer = 0f;
    private bool isGrounded = false;
    public LayerMask ground;
    
    // Variables for interacting with walls
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    public bool canWallJump = true;
    private float jumpTime = 0f;

    // Variables for dashing
    private float dashCooldown = 0f;
    [SerializeField]private bool canDash = false;

    public float moveSpeed = 5f;
    public float maxMoveSpeed = 5f;
    float horizontal;
    float vertical;
    public Rigidbody2D rb;
    //public InputActionReference moveAction;
    private Vector2 moveInput;

    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
{
    // Sprite flipping — purely visual, not physics
    if (horizontal == 1)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else if (horizontal == -1)
    {
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    // Animation state — should update every rendered frame for smoothness
    SetAnimation(horizontal);
    // end of Update
    //Debug.Log($"[Update] vX={rb.linearVelocityX}, pos={transform.position}");
}
    void FixedUpdate()
    {

        if (horizontal != 0 && rb.linearVelocityX <= maxMoveSpeed)
        {
            rb.linearVelocityX = horizontal * moveSpeed;
        }
        else if (horizontal != 0)
        {
            if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(horizontal)){
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
            if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
            {
                rb.linearVelocityX -= 10f * Mathf.Sign(rb.linearVelocityX) * Time.fixedDeltaTime;
            }
            // Otherwise, instantly stop moving
            else
            {
                rb.linearVelocityX = 0f;
            }
        }

        


        if (Physics2D.OverlapArea(new Vector2(rb.transform.position.x - 0.45f, rb.transform.position.y - 0.45f), new Vector2(rb.transform.position.x + 0.45f, rb.transform.position.y - 0.55f), ground) && jumpResetCoolDown > 0.2)
        {
            jumpCount = maxJumpCount;
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
        if (Physics2D.OverlapArea(new Vector2(rb.transform.position.x-0.5f,rb.transform.position.y+0.35f),new Vector2(rb.transform.position.x-0.4f,rb.transform.position.y-0.35f), ground))
        {
            if (isTouchingLeftWall == false && jumpCount != maxJumpCount && canWallJump && jumpTime >0.5f)
            {
                jumpCount++;
            }
            isTouchingLeftWall = true;
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
            if (isTouchingRightWall == false && jumpCount != maxJumpCount && canWallJump && jumpTime>0.5f)
            {
                jumpCount++;
            }
            isTouchingRightWall = true;
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
        // end of FixedUpdate, after all velocity-setting code
        //Debug.Log($"[FixedUpdate] vX={rb.linearVelocityX}, pos={transform.position}");
        //Debug.Log($"L={isTouchingLeftWall} R={isTouchingRightWall} grounded={isGrounded} velX={rb.linearVelocityX}");
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        horizontal = moveInput.x;
        vertical = moveInput.y;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (jumpCount > 0 || ((isTouchingLeftWall || isTouchingRightWall) && canWallJump)) 
            {
                rb.linearVelocityY = jumpforce;
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
                        rb.linearVelocityX = 10f;
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
                        rb.linearVelocityX = -10f;
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
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (dashCooldown >= 5 && canDash)
        {
            rb.linearVelocityX = transform.localScale.x*20;
            dashCooldown = 0f;
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
