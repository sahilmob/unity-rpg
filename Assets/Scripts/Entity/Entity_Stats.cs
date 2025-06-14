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
}
