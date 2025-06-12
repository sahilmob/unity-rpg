using UnityEngine;

public class Enemy_Health : Entity_Health
{

    private Enemy enemy => GetComponentInParent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer)
    {

        base.TakeDamage(damage, damageDealer);

        if (isDead)
        {
            return;
        }

        if (damageDealer.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattlerState(damageDealer);
        }

    }

}
