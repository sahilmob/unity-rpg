using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;
    private Entity entity;
    private Entity_Stats stats;
    [SerializeField] private float currentHp;
    [SerializeField] protected bool isDead;
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
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
        currentHp = stats.GetMaxHealth;
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return false;
        if (AttackEvaded()) return false;

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1 - mitigation);
        Debug.Log(finalDamage);
        Vector2 knockback = CalculateKnokback(finalDamage, damageDealer);
        float knockbackDuration = CalculateKnokbackDuration(finalDamage);
        entityVfx?.PlayOnDamageVfx();
        entity?.ReceiveKnockback(knockback, knockbackDuration);
        ReduceHp(finalDamage);

        return true;
    }

    private bool AttackEvaded()
    {
        return Random.Range(0, 100) < stats.GetEvasion;
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private Vector2 CalculateKnokback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHp / stats.GetMaxHealth;
    }

    private float CalculateKnokbackDuration(float damage)
    {

        return IsHeavyDamage(damage) ? heavyDamageDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        return damage / stats.GetMaxHealth > heavyDamageThreshold;
    }
}
