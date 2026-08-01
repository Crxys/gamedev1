using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private SpriteRenderer characterRenderer;
    private PlayerMovement player;
    [SerializeField] private LayerMask playerLayer;
    private ContactFilter2D contactFilter;
    private float currentHealth;
    [SerializeField] private Collider2D enemyCollider;
    private float paintLevel = 0f;
    public bool hasBeenHit { get; set; }
    
    void Start()
    {
        characterRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        paintLevel = 0f;
        player = FindFirstObjectByType<PlayerMovement>();
        // Configure the filter to ONLY look for the Enemy layer
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = playerLayer;
        
        // CRITICAL: Tells Unity to check overlaps even if normal collision is turned off
        contactFilter.useTriggers = true;
        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        characterRenderer.color = new Color(1f, 1f - paintLevel/5f, 1f - paintLevel/5f, 1f);
    }
    void FixedUpdate()
    {
        int playerCount = enemyCollider.Overlap(contactFilter, new Collider2D[1]);
        if(playerCount > 0 && player.isDashing > 0f)
        {
            //Debug.Log($"Enemy hit by player dash! Paint level: {paintLevel}");
            Damage(paintLevel*paintLevel/2f); // Damage is proportional to the square of the paint level
            paintLevel = 0f;
        }
    }
    public void Damage(float damageAmount)
    {
        hasBeenHit = true;
        currentHealth -= damageAmount;
        paintLevel += damageAmount;
        if(currentHealth <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
