using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float movespeed = 5f;
    public float jumpforce = 10f;
    private int jumpCount = 1;
    private int maxJumpCount = 1;
    private float jumpResetCoolDown = 1f;
    public GameObject groundCheck;
    public LayerMask ground;
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
        
        rb.linearVelocity = new Vector2(horizontal * movespeed, rb.linearVelocity.y);
        if (Physics2D.OverlapArea(new Vector2(groundCheck.transform.position.x-0.55f,groundCheck.transform.position.y + 0.5f),new Vector2(groundCheck.transform.position.x + 0.55f, groundCheck.transform.position.y - 0.1f), ground) && jumpResetCoolDown>5)
        {
            jumpCount = maxJumpCount;
        }
        jumpResetCoolDown+= Time.deltaTime;
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
                rb.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
                jumpCount -= 1;
                jumpResetCoolDown = 0;
            }
            
        }
    }

}
