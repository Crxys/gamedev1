using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float movespeed = 5f;
    public float jumpforce = 10f;
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
            rb.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
        }
    }
}
