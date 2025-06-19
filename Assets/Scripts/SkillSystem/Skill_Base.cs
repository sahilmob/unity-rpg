using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    private bool OnCooldown => Time.time < lastTimeUsed + cooldown;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnCooldown)
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    public void SetSkillOnCooldown()
    {
        lastTimeUsed = Time.time;
    }
    public void RestCooldownBy(float cooldownReduction)
    {
        lastTimeUsed = lastTimeUsed + cooldownReduction;
    }
    public void ResetCooldown()
    {
        lastTimeUsed = Time.time;
    }
}
