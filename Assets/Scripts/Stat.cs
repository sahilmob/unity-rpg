using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] float baseValue;

    public float value
    {
        get => baseValue;
    }
}
