using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        entity.OnFlipped -= HandleFlip;
    }
    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
