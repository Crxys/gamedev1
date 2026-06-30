using Unity.VisualScripting;
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
    public GameObject groundCheck;
    public LayerMask ground;
    
    // Variables for interacting with walls
    private bool isTouchingLeftWall = false;
    public GameObject leftWallCheck;
    private bool isTouchingRightWall = false;
    public GameObject rightWallCheck;
    public bool canWallJump = true;

    // Variables for dashing
    private float dashCooldown = 0f;
    private bool canDash = true;

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
        if (Mathf.Abs(rb.linearVelocityX) + moveSpeed <= maxMoveSpeed)
        {
            rb.linearVelocityX += moveSpeed * horizontal;
        }

        else if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
        {
            rb.linearVelocityX -= 1f * Mathf.Sign(rb.linearVelocityX);
        }

        else
        {
            rb.linearVelocityX = maxMoveSpeed * horizontal;
        }

        if (horizontal == 1)
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
        else if (horizontal == -1)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }


        if (Physics2D.OverlapArea(new Vector2(groundCheck.transform.position.x - 0.45f, groundCheck.transform.position.y + 0.05f), new Vector2(groundCheck.transform.position.x + 0.45f, groundCheck.transform.position.y - 0.05f), ground) && jumpResetCoolDown > 0.2)
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
        if (Physics2D.OverlapArea(new Vector2(leftWallCheck.transform.position.x-0.05f,leftWallCheck.transform.position.y+0.35f),new Vector2(leftWallCheck.transform.position.x+0.05f,leftWallCheck.transform.position.y-0.35f), ground))
        {
            if (isTouchingLeftWall == false && jumpCount != maxJumpCount && canWallJump)
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

        if (Physics2D.OverlapArea(new Vector2(rightWallCheck.transform.position.x - 0.05f, rightWallCheck.transform.position.y + 0.35f), new Vector2(rightWallCheck.transform.position.x + 0.05f, rightWallCheck.transform.position.y - 0.35f), ground))
        {
            if (isTouchingRightWall == false && jumpCount != maxJumpCount && canWallJump)
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
        jumpResetCoolDown += Time.deltaTime;
        dashCooldown += Time.deltaTime;
        SetAnimation(horizontal);
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
                    rb.linearVelocityX = 20f;
                    isTouchingLeftWall = false;
                }
                if (isTouchingRightWall && canWallJump && isTouchingLeftWall == false)
                {
                    rb.linearVelocityX = -20f;
                    isTouchingRightWall = false;
                }
            }
            
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (dashCooldown >= 5 && canDash)
        {
            rb.linearVelocityX = transform.localScale.x*50;
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
