using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType buffType;
    public float buffValue;
}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats statsToModify;

    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private float buffDuration = 4;
    [SerializeField] private bool canBeUsed = true;
    [SerializeField] private string buffName;

    [Header("Floaty Movement")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = .1f;
    private Vector3 startPosition;


    void Awake()
    {
        startPosition = transform.position;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed) return;

        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;

        Debug.Log("Buff is applied");
        foreach (Buff b in buffs)
        {
            statsToModify.GetStatByType(b.buffType).AddModifier(b.buffValue, buffName);
        }

        yield return new WaitForSeconds(duration);

        foreach (Buff b in buffs)
        {
            statsToModify.GetStatByType(b.buffType).RemoveModifier(buffName);
            Debug.Log("Buff is removed");
            Destroy(gameObject);
        }
    }
}
