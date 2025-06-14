using UnityEngine;

public class Enemy_Health : Entity_Health
{

    private Enemy enemy => GetComponentInParent<Enemy>();

    public override bool TakeDamage(float damage, float elementalDamage, Transform damageDealer)
    {

        bool tookDamage = base.TakeDamage(damage, elementalDamage, damageDealer);

        if (!tookDamage)
        {
            return false;
        }

        if (damageDealer.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattlerState(damageDealer);
        }

        return true;

    }

}
