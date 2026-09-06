using UnityEngine;

public interface IDamageable
{
    public void Damage(float damageAmount);

    public void Die();

    float maxHealth { get; set; }
    float currentHealth { get; set; }

}
