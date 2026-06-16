using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static int enemyCount = 0;
    private int myID;
    private Rigidbody2D me;
    public GameObject player;
    private float jumpforce = 10f;
    private float maxMoveSpeed = 5f;
    private bool isGrounded = false;
    public LayerMask ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = GetComponent<Rigidbody2D>();
        myID = enemyCount;
        enemyCount++;
        Debug.Log("Loaded Enemy #" + myID);
    }

    // Update is called once per frame
    void Update()
    {
        float xToPlayer = player.transform.position.x-me.transform.position.x;
        me.linearVelocityX += Mathf.Sign(xToPlayer);
        if (isGrounded&&Mathf.Abs(xToPlayer)<5) Jump();
        //friction
        me.linearVelocityX *= 0.8f;

        if (Physics2D.OverlapArea(new Vector2(me.transform.position.x-0.5f,me.transform.position.y + 0.5f),new Vector2(me.transform.position.x + 0.5f, me.transform.position.y - 0.5f), ground))
             {isGrounded = true;}
        else {isGrounded = false;}
    }

    public void Jump()
    {
        me.linearVelocityY = jumpforce;
        isGrounded = false;
    }

}
