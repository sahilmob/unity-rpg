using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    public float damage = 10;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius = 1;

    void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {

        foreach (Collider2D c in detectedColliders)
        {
            IDamageable damageable = c.GetComponent<IDamageable>();
            if (damageable == null) continue;
            damageable.TakeDamage(damage, transform);
            vfx?.CreateOnHitVfx(c.transform);
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
