using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius = 1;

    void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {

        foreach (Collider2D c in detectedColliders)
        {
            IDamageable damageable = c.GetComponent<IDamageable>();
            if (damageable == null) continue;
            float elementalDamage = stats.GetElementalDamage;
            float damage = stats.GetPhysicalDamage(out bool isCrit);
            bool tookDamage = damageable.TakeDamage(damage, elementalDamage, transform);
            if (tookDamage)
                vfx?.CreateOnHitVfx(c.transform, isCrit);
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
