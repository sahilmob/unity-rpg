using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats stats;
    private Entity_Health health;
    private ElementType currentEffect = ElementType.None;

    void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        health = GetComponent<Entity_Health>();
        entityVfx = GetComponent<Entity_VFX>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);
        StartCoroutine(ChilledEffectCo(reducedDuration, slowMultiplier));
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator ChilledEffectCo(float duration, float multiplier)
    {
        currentEffect = ElementType.Ice;
        entity.SlowDownEntityBy(duration, multiplier);
        entityVfx.PlayerOnStatusVfx(duration, currentEffect);
        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayerOnStatusVfx(duration, ElementType.Fire);


        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            health.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
