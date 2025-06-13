using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius = 1;

    public void PerformAttack()
    {

        foreach (Collider2D c in detectedColliders)
        {
            IDamageable damageable = c.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage, transform);
        }
    }

    protected Collider2D[] detectedColliders
    {
        get
        {
            return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
