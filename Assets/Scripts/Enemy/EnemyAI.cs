using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static int enemyCount = 0;
    private int myID;
    private Rigidbody2D me;
    public PlayerMovement player;
    private float jumpforce = 10f;
    private float maxMoveSpeed = 5f;
    private bool isGrounded = false;
    public LayerMask ground;
    private EnemyKnockback enemyPhysics;

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
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapArea(new Vector2(me.transform.position.x - 0.45f, me.transform.position.y - 0.5f), 
                                 new Vector2(me.transform.position.x + 0.45f, me.transform.position.y - 0.5f), ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (enemyPhysics != null && enemyPhysics.isKnockbackActive)
        {
            return; 
        }

        if (player == null) return;

        float xToPlayer = player.transform.position.x - me.transform.position.x;
        
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
