using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats stats;
    private Entity_Health health;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        health = GetComponent<Entity_Health>();
        entityVfx = GetComponent<Entity_VFX>();
    }

    public void ApplyElectrifyEffect(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightening);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge = currentCharge + finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }

        if (electrifyCo != null)
        {
            StopCoroutine(electrifyCo);
        }

        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));

    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightening;
        entityVfx.PlayerOnStatusVfx(duration, ElementType.Lightening);

        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
        health.ReduceHp(damage);
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
        if (element == ElementType.Lightening && currentEffect == ElementType.Lightening)
        {
            return true;
        }

        return currentEffect == ElementType.None;
    }
}
