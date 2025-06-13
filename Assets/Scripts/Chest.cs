using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    public void TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = new Vector2(0, 3);
        rb.angularVelocity = Random.Range(-200, 200);
    }
}
