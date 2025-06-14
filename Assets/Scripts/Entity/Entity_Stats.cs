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
}
