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

    public float moveSpeed = 5f;
    float horizontal;
    float vertical;
    public Rigidbody2D rb;
    //public InputActionReference moveAction;
    private Vector2 moveInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        if (Physics2D.OverlapArea(new Vector2(groundCheck.transform.position.x-0.45f,groundCheck.transform.position.y + 0.1f),new Vector2(groundCheck.transform.position.x + 0.45f, groundCheck.transform.position.y - 0.1f), ground) && jumpResetCoolDown>0.2)
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
        if (Physics2D.OverlapArea(new Vector2(leftWallCheck.transform.position.x-0.05f,leftWallCheck.transform.position.y+0.45f),new Vector2(leftWallCheck.transform.position.x+0.05f,leftWallCheck.transform.position.y-0.45f), ground))
        {
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

        if (Physics2D.OverlapArea(new Vector2(rightWallCheck.transform.position.x - 0.05f, rightWallCheck.transform.position.y + 0.45f), new Vector2(rightWallCheck.transform.position.x + 0.05f, rightWallCheck.transform.position.y - 0.45f), ground))
        {
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
            if (jumpCount > 0) {
                rb.linearVelocityY = jumpforce;
                jumpCount -= 1;
                isGrounded = false;
                jumpResetCoolDown = 0;
                if (isTouchingLeftWall)
                {
                    rb.linearVelocityX = 10f;
                    isTouchingLeftWall = false;
                }
                if (isTouchingRightWall)
                {
                    rb.linearVelocityX = -10f;
                    isTouchingRightWall = false;
                }
            }
            
        }
    }

}
