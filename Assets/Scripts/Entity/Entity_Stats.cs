using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatsSetup;
    public Stat_ResourceGroup resources;
    public Stat_OffensiveGroup offense;
    public Stat_DefensiveGroup defense;
    public Stat_MajorGroup major;

    public float GetMaxHealth
    {
        get
        {
            float baseHp = resources.maxHealth.value;
            float bonusHp = major.vitality.value * 5;

            return baseHp + bonusHp;
        }
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
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

        return finalDamage * scaleFactor;
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

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {

        float fireDamage = offense.fireDamage.value;
        float iceDamage = offense.iceDamage.value;
        float lightningDamage = offense.lightningDamage.value;

        float bonusElementalDamage = major.intelligence.value;
        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightening;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = fireDamage == highestDamage ? 0 : fireDamage * 0.5f;
        float bonusIce = iceDamage == highestDamage ? 0 : iceDamage * 0.5f;
        float bonusLightening = lightningDamage == highestDamage ? 0 : lightningDamage * 0.5f;

        float weakerElementsDamage = bonusFire + bonusIce + bonusLightening;
        float finalDamage = highestDamage + bonusElementalDamage + weakerElementsDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = element switch
        {
            ElementType.Fire => defense.fireRes.value,
            ElementType.Ice => defense.iceRes.value,
            ElementType.Lightening => defense.lightningRes.value,
            _ => 0,
        };

        float bonusResistance = major.intelligence.value * 0.5f;

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        return Mathf.Clamp(resistance, 0, resistanceCap) / 100;
    }

    public Stat GetStatByType(StatType type)
    {
        return type switch
        {
            StatType.MaxHealth => resources.maxHealth,
            StatType.HealthRegen => resources.healthRegen,
            StatType.Strength => major.strength,
            StatType.Agility => major.agility,
            StatType.Intelligence => major.intelligence,
            StatType.Vitality => major.vitality,
            StatType.AttackSpeed => offense.attackSpeed,
            StatType.Damage => offense.damage,
            StatType.CritChance => offense.critChance,
            StatType.CritPower => offense.critPower,
            StatType.ArmorReduction => offense.armorReduction,
            StatType.FireDamage => offense.fireDamage,
            StatType.IceDamage => offense.iceDamage,
            StatType.LightningDamage => offense.lightningDamage,
            StatType.Armor => defense.armor,
            StatType.Evasion => defense.evasion,
            StatType.IceResistance => defense.iceRes,
            StatType.FireResistance => defense.fireRes,
            StatType.LightningResistance => defense.lightningRes,
            _ => default
        };
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatsSetup == null) return;
        resources.maxHealth.SetBaseValue(defaultStatsSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatsSetup.healthRegen);
        major.strength.SetBaseValue(defaultStatsSetup.strength);
        major.agility.SetBaseValue(defaultStatsSetup.agility);
        major.intelligence.SetBaseValue(defaultStatsSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatsSetup.vitality);
        offense.attackSpeed.SetBaseValue(defaultStatsSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatsSetup.damage);
        offense.critChance.SetBaseValue(defaultStatsSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatsSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatsSetup.armorReduction);
        offense.fireDamage.SetBaseValue(defaultStatsSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatsSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatsSetup.lightningDamage);
        defense.armor.SetBaseValue(defaultStatsSetup.armor);
        defense.evasion.SetBaseValue(defaultStatsSetup.evasion);
        defense.fireRes.SetBaseValue(defaultStatsSetup.fireRes);
        defense.iceRes.SetBaseValue(defaultStatsSetup.iceRes);
        defense.lightningRes.SetBaseValue(defaultStatsSetup.lightningRes);
    }
}
