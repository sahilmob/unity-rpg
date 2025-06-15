using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCo;
    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;
    [SerializeField] private Color hitVfxColor = Color.white;
    [Header("Element Colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    private Color originalHitVfxColor;


    void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCo != null)
        {
            StopCoroutine(onDamageVfxCo);
        }

        onDamageVfxCo = StartCoroutine(OnDamageVfxCo());
    }

    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.Ice)
            hitVfxColor = chillVfx;

        if (element == ElementType.None)
            hitVfxColor = originalHitVfxColor;
    }

    public void PlayerOnStatusVfx(float duration, ElementType element)
    {
        Color color = element switch
        {
            ElementType.Ice => chillVfx,
            ElementType.Fire => burnVfx,
            _ => Color.white
        };

        StartCoroutine(PlayerStatusVfxCo(duration, color));
    }

    private IEnumerator PlayerStatusVfxCo(float duration, Color color)
    {
        float tickInterval = .25f;
        float timePassed = 0;

        Color lightColor = color * 1.2f;
        Color darkColor = color * .8f;

        bool toggle = false;

        while (timePassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            yield return new WaitForSeconds(tickInterval);
            timePassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
        if (entity.facingDir == 1 && isCrit)
        {
            vfx.transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
