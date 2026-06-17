using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;

    private RaycastHit2D[] hits;
    // Update is called once per frame
    private Vector2 moveInput;
    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Attack performed");
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();
                if(damageable != null)            {
                    damageable.Damage(damageAmount);
                }
            }
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
