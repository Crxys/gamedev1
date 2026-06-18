using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isKnockbackActive { get; private set; } = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction, float force, float duration)
    {
        if (isKnockbackActive) return; 

        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        isKnockbackActive = true;
        rb.linearVelocity = direction.normalized * force;

        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero; 
        isKnockbackActive = false;
    }
}
