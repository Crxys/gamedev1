using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Visual & Audio")]
    public string attackName;
    public AnimationClip animationClip;
    public GameObject impactVFX;
    public AudioClip impactSFX;

    [Header("Combat Stats")]
    public float damage = 1f;
    public float baseKnockbackForce = 18f;
    public float baseKnockbackDuration = 0.25f;
    public float attackRange = 1.5f;  
    public float swingDuration = 0.1f; 
    public float swingCooldown = 0.2f;

}
