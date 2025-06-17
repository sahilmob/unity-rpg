using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    private float finalValue;
    private bool recalculate = true;
    [SerializeField] float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    public float value
    {
        get
        {
            if (!recalculate) return this.finalValue;

            float finalValue = baseValue;
            foreach (StatModifier mod in modifiers)
            {
                finalValue += mod.value;
            }
            this.finalValue = finalValue;
            recalculate = false;
            return this.finalValue; ;
        }
    }


    public void AddModifier(float value, string source)
    {
        StatModifier mod = new StatModifier(value, source);
        modifiers.Add(mod);
        recalculate = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(m => m.source == source);
        recalculate = true;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
