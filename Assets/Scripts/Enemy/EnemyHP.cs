using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private SpriteRenderer characterRenderer;

    private float currentHealth;
    private float paintLevel = 0f;
    public bool hasBeenHit { get; set; }

    void Start()
    {
        characterRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        paintLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        characterRenderer.color = new Color(1f, 1f - paintLevel/5f, 1f - paintLevel/5f, 1f);
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
