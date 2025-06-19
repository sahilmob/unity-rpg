using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneStartAndArrival))
        {
            CreateClone();
        }

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStart) || Unlocked(SkillUpgradeType.Dash_ShardStartAndArrival))
        {
            CreateShard();
        }
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneStartAndArrival))
        {
            CreateClone();
        }

        if (Unlocked(SkillUpgradeType.Dash_ShardStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        Debug.Log("Create shard");
    }

    private void CreateClone()
    {
        Debug.Log("Create Echo");
    }
}
