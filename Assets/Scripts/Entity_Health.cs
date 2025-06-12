using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] public float maxHp = 100;
    [SerializeField] protected bool isDead;
    private Entity_VFX entityVfx;
    private Entity entity;
    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("On Heavy Damage")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = 0.3f;
    [SerializeField] private float heavyDamageDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);




    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return;
        Vector2 knockback = CalculateKnokback(damage, damageDealer);
        float knockbackDuration = CalculateKnokbackDuration(damage);
        entityVfx?.PlayOnDamageVfx();
        entity?.ReceiveKnockback(knockback, knockbackDuration);
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if (maxHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Entity died");
    }

    private Vector2 CalculateKnokback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateKnokbackDuration(float damage)
    {

        return IsHeavyDamage(damage) ? heavyDamageDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        return damage / maxHp > heavyDamageThreshold;
    }
}
