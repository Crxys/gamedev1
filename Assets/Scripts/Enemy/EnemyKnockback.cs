using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Coroutine currentKnockbackCoroutine;

    // You can still keep this property if other scripts need to check it!
    public bool isKnockbackActive => currentKnockbackCoroutine != null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction, float force, float duration)
    {
        // If already flying back, stop that timer/velocity control immediately
        if (currentKnockbackCoroutine != null)
        {
            StopCoroutine(currentKnockbackCoroutine);
            
        }

        // Start the new knockback and store the reference
        currentKnockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        rb.linearVelocity = direction.normalized * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero; 
        currentKnockbackCoroutine = null; // Clear the reference when finished
    }
}