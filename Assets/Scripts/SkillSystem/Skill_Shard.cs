using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        SkillObject_Shard shardObject = shard.GetComponent<SkillObject_Shard>();
        shardObject.SetupShard(detonateTime);
    }
}
