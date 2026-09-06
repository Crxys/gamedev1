using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable, IEnemyMovable
{
    public float currentHealth { get; set; } = 10f;
    public float maxHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public LayerMask ground;

    public EnemyStateMachine enemyStateMachine { get; set; }
    public EnemyPacingIdle enemyPacingIdle { get; set; }

    public void Awake()
    {
        enemyStateMachine = new EnemyStateMachine();
        enemyPacingIdle = new EnemyPacingIdle(this,enemyStateMachine);
    }

    public void Start()
    {
        currentHealth = maxHealth;
        RB = GetComponent<Rigidbody2D>();

        enemyStateMachine.initialize(enemyPacingIdle);
    }
    public void Damage( float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
    public void MoveEnemy(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
    }
    public void Update()
    {
        enemyStateMachine.currentEnemyState.FrameUpdate();
    }
    public void FixedUpdate()
    {
        enemyStateMachine.currentEnemyState.PhysicsUpdate();
    }
}
