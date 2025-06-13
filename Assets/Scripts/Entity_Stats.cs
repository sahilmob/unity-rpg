using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat vitality;

    public float GetMaxHealth
    {
        get
        {
            float baseHp = maxHealth.value;
            float bonusHp = vitality.value * 5;

            return baseHp + bonusHp;
        }
    }
}
