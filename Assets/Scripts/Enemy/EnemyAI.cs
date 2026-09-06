using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static int enemyCount = 0;
    private int myID;
    private Rigidbody2D me;
    public PlayerMovement player;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float size = 0.25f;
    private bool isGrounded = false;
    public LayerMask ground;
    private EnemyKnockback enemyPhysics;
    private float horizontal = 0f;
    

    void Start()
    {
        enemyPhysics = GetComponent<EnemyKnockback>();
        me = GetComponent<Rigidbody2D>();
        myID = enemyCount;
        enemyCount++;
        player = Object.FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(size, size, 1f);
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(-size, size, 1f);
        }
        
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapArea(new Vector2(me.transform.position.x - 0.4f, me.transform.position.y - 0.5f), new Vector2(me.transform.position.x + 0.4f, me.transform.position.y - 0.7f), ground))
        {
            isGrounded = true;
            
        }
        else
        {
            isGrounded = false;
        }

        if (enemyPhysics != null && enemyPhysics.isKnockbackActive)
        {
            //Debug.Log($"Knockback active, skipping movement logic. {enemyPhysics.isKnockbackActive}");
            return; 
        }

        if (player == null) return;

        float xToPlayer = player.transform.position.x - me.transform.position.x;
        if (Mathf.Abs(xToPlayer) > 0.1f)
        {
            horizontal = Mathf.Sign(xToPlayer);
        }
        else
        {
            horizontal = 0f;
        }
        me.linearVelocityX += Mathf.Sign(xToPlayer) * 60f * Time.fixedDeltaTime;
        me.linearVelocityX *= Mathf.Pow(1 - 1 / maxMoveSpeed, Time.fixedDeltaTime * 60f);
        
        if (isGrounded && (Mathf.Abs(xToPlayer) < 5 || Random.value < 0.15f * Time.fixedDeltaTime || (Mathf.Abs(xToPlayer) > 0.5f && Mathf.Abs(me.linearVelocityX) < 0.05f)))
        {
            Jump();
        }
        
        
    }
    void OnDrawGizmos()
    {
        // 1. Ground Check (Green)
        Gizmos.color = Color.green;
        Vector2 groundA = new Vector2(transform.position.x - 0.4f, transform.position.y - 0.5f);
        Vector2 groundB = new Vector2(transform.position.x + 0.4f, transform.position.y - 0.7f);
        DrawOverlapBox(groundA, groundB);

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
    public void Jump()
    {
        me.linearVelocityY = jumpforce;
        isGrounded = false;
    }
}
