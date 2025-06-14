using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DefensiveGroup defense;

    public float GetMaxHealth
    {
        get
        {
            float baseHp = maxHealth.value;
            float bonusHp = major.vitality.value * 5;

            return baseHp + bonusHp;
        }
    }

    public float GetPhysicalDamage(out bool isCrit)
    {

        float baseDamage = offense.damage.value;
        float bonusDamage = major.strength.value;
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.value;
        float bonusCritChance = major.agility.value * .3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.value;
        float bonusCritPower = major.strength.value * .5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = UnityEngine.Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage;
    }
    public float GetEvasion
    {
        get
        {
            float baseEvasion = defense.evasion.value;
            float bonusEvasion = major.agility.value * .5f;
            float evasionCap = 85;

            return Math.Clamp(baseEvasion + bonusEvasion, 0, evasionCap);
        }
    }

    public float GetArmorMitigation(float armorReduction)
    {

        float baseArmor = defense.armor.value;
        float bonusArmor = major.agility.value;
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveMultiplier = totalArmor * reductionMultiplier;

        float mitigation = effectiveMultiplier / (effectiveMultiplier + 100);
        float mitigationCap = 0.85f;
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction
    {
        get
        {
            float finalReduction = offense.armorReduction.value / 100;

            return finalReduction;
        }
    }

    public float GetElementalDamage
    {
        get
        {
            float fireDamage = offense.fireDamage.value;
            float iceDamage = offense.iceDamage.value;
            float lightningDamage = offense.lightningDamage.value;

            float bonusElementalDamage = major.intelligence.value;
            float highestDamage = Mathf.Max(fireDamage, iceDamage, lightningDamage);

            if (highestDamage <= 0)
            {
                return 0;
            }

            float bonusFire = fireDamage == highestDamage ? 0 : fireDamage * 0.5f;
            float bonusIce = iceDamage == highestDamage ? 0 : iceDamage * 0.5f;
            float bonusLightening = lightningDamage == highestDamage ? 0 : lightningDamage * 0.5f;

            float weakerElementsDamage = bonusFire + bonusIce + bonusLightening;
            float finalDamage = highestDamage + bonusElementalDamage + weakerElementsDamage;

            return finalDamage;
        }
    }
}
