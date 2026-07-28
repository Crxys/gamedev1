using UnityEngine;

public interface IDamageable
{
    public void Damage(float damageAmount);
    public bool hasBeenHit { get; set; }
}
