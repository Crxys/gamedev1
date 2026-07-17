using UnityEngine;
using UnityEngine.Events;
public class playerHealth : MonoBehaviour
{
    public delegate void playerDeath();
    public static event playerDeath OnDeath;
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    private bool alive = true;

    [SerializeField] private LayerMask enemyLayer; // Set this to "Enemy" in the Inspector
    
    private Collider2D playerCollider;
    private ContactFilter2D contactFilter;
    private Collider2D[] results = new Collider2D[10]; // Stores overlapping enemies

    public bool hasBeenHit { get; set; }

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 1.0f; 
    [SerializeField] private float invincibilityAlpha = 0.5f;
    [SerializeField] private SpriteRenderer playerSprite; // Assign the player's sprite renderer in the
    private float cooldownTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        // Get the player's physical 2D collider
        playerCollider = GetComponent<Collider2D>();

        // Configure the filter to ONLY look for the Enemy layer
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = enemyLayer;
        
        // CRITICAL: Tells Unity to check overlaps even if normal collision is turned off
        contactFilter.useTriggers = true;

        PlayerMovement.playerDashed += temporaryInvulnerability;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            // Optional visual flash/fade effect
            if (playerSprite != null)
            {
                Color c = playerSprite.color;
                c.a = invincibilityAlpha;
                playerSprite.color = c;
            }

            // If the timer just finished, restore full visibility
            if (cooldownTimer <= 0 && playerSprite != null)
            {
                Color c = playerSprite.color;
                c.a = 1f;
                playerSprite.color = c;
            }
        }
    }


    public void Damage(float damageAmount)
    {
        hasBeenHit = true;
        currentHealth -= damageAmount;
        if(currentHealth <= 0f)
        {
            OnDeath.Invoke();
            alive = false;
        }
    }
    

    void FixedUpdate()
    {
        if (cooldownTimer > 0) return;
        int enemyCount = playerCollider.Overlap(contactFilter, results);

        if (enemyCount > 0)
        {
            // The player is currently touching at least one enemy clone
            cooldownTimer = invincibilityDuration; // Start the invincibility timer
            Debug.Log("Took damage!");
            //Damage(1f); //change based off of enemy damage
            
        }
    }
   
    void temporaryInvulnerability()
    {
        cooldownTimer = 1.75f;
    }
}
