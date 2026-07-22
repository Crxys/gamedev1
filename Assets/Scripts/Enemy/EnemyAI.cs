using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static int enemyCount = 0;
    private int myID;
    private Rigidbody2D me;
    public PlayerMovement player;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private float maxMoveSpeed = 5f;
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
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        }
        
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapArea(new Vector2(me.transform.position.x - 0.45f, me.transform.position.y - 0.35f), new Vector2(me.transform.position.x + 0.45f, me.transform.position.y - 0.55f), ground))
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

    public void Jump()
    {
        me.linearVelocityY = jumpforce;
        isGrounded = false;
    }
}
