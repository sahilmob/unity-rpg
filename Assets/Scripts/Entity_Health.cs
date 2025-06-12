using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] public float maxHp = 100;
    [SerializeField] protected bool isDead;
    private Entity_VFX entityVfx;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return;
        entityVfx?.PlayOnDamageVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if (maxHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Entity died");
    }
}
