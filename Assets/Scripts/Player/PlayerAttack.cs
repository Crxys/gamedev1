using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections.Generic;
using System.Collections;
public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    public float attackCooldown = 0.1f;
    public bool isAttacking { get; private set; } = false;
    private Animator animator;
    private RaycastHit2D[] hits;
    // Update is called once per frame
    private Vector2 moveInput;
    private float attackTimer = 1f;
    private List<IDamageable> damagedObjects = new List<IDamageable>();

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        attackTimer += Time.deltaTime;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed && attackTimer >= attackCooldown)
        {
            //performAttack();
            animator.SetTrigger("attack");
            attackTimer = 0f;
        }
    }
    
    public IEnumerator DamageWhileSlashIsActive()
    {
        isAttacking = true;
        while(isAttacking)
        {
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();
                if(damageable != null && !damageable.hasBeenHit)            
                {
                    damageable.Damage(damageAmount);
                    damagedObjects.Add(damageable);
                }
            }
            yield return null;
        }
        ReturnAttackableObjectsToNormal();
    }
    public void ReturnAttackableObjectsToNormal()
    {
        foreach(IDamageable damageable in damagedObjects)
        {
            damageable.hasBeenHit = false;
        }
        damagedObjects.Clear();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
    #region Animation Triggers

    public void ShouldBeDamagingToFalse()
    {
        isAttacking = false;
    }


    #endregion
}
