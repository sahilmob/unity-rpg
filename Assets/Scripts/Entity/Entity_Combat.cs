using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float targetCheckRadius = 1;
    [Header("Status Effect Details")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillSlowMultiplier = .2f;


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
            float elementalDamage = stats.GetElementalDamage(out ElementType element, .6f);
            float damage = stats.GetPhysicalDamage(out bool isCrit);
            bool tookDamage = damageable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
            {
                ApplyStatusEffect(c.transform, element);
            }

            if (tookDamage)
            {
                vfx?.UpdateOnHitColor(element);
                vfx?.CreateOnHitVfx(c.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null) return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(element))
        {
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier);
        }

        if (element == ElementType.Fire && statusHandler.CanBeApplied(element))
        {
            float fireDamage = stats.offense.fireDamage.value * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
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
