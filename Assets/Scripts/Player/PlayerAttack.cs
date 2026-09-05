using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon Transform Hooks")]
    public Transform weaponPivot;
    //public float attackRange = 1.5f;    
    public LayerMask enemyLayers;       
    //public float attackDamage = 1.0f;   
    
    //[Header("Knockback Settings")]
    //public float baseKnockbackForce = 18f;
    //public float baseKnockbackDuration = 0.25f;

    //[Header("Scripted Animation Properties")]
    //public float swingAngle = 110f;
    //public float swingDuration = 0.1f; 
    private Transform automaticAttackPoint; 
    private bool isSwinging = false;
    //private Quaternion originalRotation;

    //[SerializeField] private float swingCooldown = 0.2f;
    private float swingCooldownTimer = 0f;
    public AttackData Default; // Reference to the AttackData ScriptableObject
    void Start()
    {
        automaticAttackPoint = transform.Find("AttackPoint");
        /*
        if (weaponPivot != null)
        {
            originalRotation = weaponPivot.localRotation;
        }
        else
        {
            Debug.LogError("ATTACK ERROR: Please drag your WeaponPivot object into the inspector slot!");
        }

        if (automaticAttackPoint == null)
        {
            Debug.LogError("ATTACK ERROR: Could not find a child GameObject named exactly 'AttackPoint'!");
        }
        */
    }

    public void Attack()
    {
        if (!isSwinging)
        {
            StartCoroutine(ProceduralSwingRoutine());
        }
    }

    private IEnumerator ProceduralSwingRoutine(AttackData attackData = null)
    {
        if (attackData == null)
        {
            attackData = Default; // Use the default AttackData if none is provided
        }

        float attackDamage = attackData.damage;
        float attackRange = attackData.attackRange;
        float swingDuration = attackData.swingDuration;
        float swingCooldown = attackData.swingCooldown;
        float baseKnockbackForce = attackData.baseKnockbackForce;
        float baseKnockbackDuration = attackData.baseKnockbackDuration;

        if (swingCooldownTimer > 0f)
        {
            yield break; // Exit if still in cooldown
        }
    
        isSwinging = true;

        PerformMeleeAttack(attackData);

        float elapsedTime = 0f;
        
        //Quaternion startRot = originalRotation * Quaternion.Euler(0, 0, swingAngle / 2f);
        //Quaternion endRot = originalRotation * Quaternion.Euler(0, 0, -swingAngle / 2f);

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            //float percentage = elapsedTime / swingDuration;

            //float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentage);

            if (weaponPivot != null)
            {
                //weaponPivot.localRotation = Quaternion.Slerp(startRot, endRot, smoothPercentage);
            }
            PerformMeleeAttack(attackData);
            yield return null;
        }
        
        elapsedTime = 0f;
        float returnDuration = 0.1f;
        //Quaternion currentRot = weaponPivot.localRotation;

        while (elapsedTime < returnDuration)
        {
            elapsedTime += Time.deltaTime;
            //weaponPivot.localRotation = Quaternion.Slerp(currentRot, originalRotation, elapsedTime / returnDuration);
            yield return null;
        }
        
        //weaponPivot.localRotation = originalRotation;
        swingCooldownTimer = swingCooldown;
        while(swingCooldownTimer > 0f)
        {
            swingCooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isSwinging = false;
    }

    public void PerformMeleeAttack(AttackData attack = null)
    {
        if (attack == null)
        {
            attack = Default;
        }

        if (automaticAttackPoint == null) return;

        Vector3 searchPosition = automaticAttackPoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(searchPosition, attack.attackRange, enemyLayers);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyKnockback kb = enemyCollider.GetComponent<EnemyKnockback>();
            if (kb != null)
            {
                Vector3 enemyPos = enemyCollider.transform.position;
                Vector3 playerPos = transform.position;
                enemyPos.y = 0;
                playerPos.y = 0;

                Vector2 direction = (enemyPos - playerPos).normalized;
                direction.x = Mathf.Abs(direction.x) * Mathf.Sign(transform.localScale.x);
                kb.Knockback(direction, attack.baseKnockbackForce, attack.baseKnockbackDuration);
            }

            EnemyHP enemyHealth = enemyCollider.GetComponent<EnemyHP>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(attack.damage);
                Debug.Log($"{enemyCollider.name} took {attack.damage} damage procedurally!");
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (automaticAttackPoint != null)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(automaticAttackPoint.position, attackRange);
        }
    }
    
}
