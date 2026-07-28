using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon Transform Hooks")]
    public Transform weaponPivot;
    public float attackRange = 1.5f;    
    public LayerMask enemyLayers;       
    public float attackDamage = 1.0f;   

    [Header("Knockback Settings")]
    public float baseKnockbackForce = 18f;
    public float baseKnockbackDuration = 0.25f;

    [Header("Scripted Animation Properties")]
    public float swingAngle = 110f;
    public float swingDuration = 0.1f; 
    private Transform automaticAttackPoint; 
    private bool isSwinging = false;
    private Quaternion originalRotation;

    void Start()
    {
        automaticAttackPoint = transform.Find("AttackPoint");

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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(ProceduralSwingRoutine());
        }
    }

    private IEnumerator ProceduralSwingRoutine()
    {
        isSwinging = true;

        PerformMeleeAttack();

        float elapsedTime = 0f;
        
        Quaternion startRot = originalRotation * Quaternion.Euler(0, 0, swingAngle / 2f);
        Quaternion endRot = originalRotation * Quaternion.Euler(0, 0, -swingAngle / 2f);

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / swingDuration;

            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentage);

            if (weaponPivot != null)
            {
                weaponPivot.localRotation = Quaternion.Slerp(startRot, endRot, smoothPercentage);
            }

            yield return null;
        }

        elapsedTime = 0f;
        float returnDuration = 0.1f;
        Quaternion currentRot = weaponPivot.localRotation;

        while (elapsedTime < returnDuration)
        {
            elapsedTime += Time.deltaTime;
            weaponPivot.localRotation = Quaternion.Slerp(currentRot, originalRotation, elapsedTime / returnDuration);
            yield return null;
        }

        weaponPivot.localRotation = originalRotation;
        isSwinging = false;
    }

    public void PerformMeleeAttack()
    {
        if (automaticAttackPoint == null) return;

        Vector3 searchPosition = automaticAttackPoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(searchPosition, attackRange, enemyLayers);

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
                kb.Knockback(direction, baseKnockbackForce, baseKnockbackDuration);
            }

            EnemyHP enemyHealth = enemyCollider.GetComponent<EnemyHP>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(attackDamage);
                Debug.Log($"{enemyCollider.name} took {attackDamage} damage procedurally!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (automaticAttackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(automaticAttackPoint.position, attackRange);
        }
    }
}
