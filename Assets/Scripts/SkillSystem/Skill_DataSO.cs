using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public SkillUpgradeType upgradeType;
    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
}
