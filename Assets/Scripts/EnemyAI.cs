using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static int enemyCount = 0;
    private int myID;
    public Rigidbody2D me;
    public GameObject player;
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
        me.linearVelocityY = 1;
    }
}
