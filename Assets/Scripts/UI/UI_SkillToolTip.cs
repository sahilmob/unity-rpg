using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI_SkillTree skillTree;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRegiments;
    [SerializeField] private string metConditionsHex;
    [SerializeField] private string notMetConditionsHex;
    [SerializeField] private string infoHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is now locked.";

    override protected void Awake()
    {
        base.Awake();
        skillTree = GetComponentInParent<UI_SkillTree>();
    }

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform rect, UI_TreeNode node)
    {
        base.ShowToolTip(show, rect);

        if (show == false) return;
        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.description;

        string skillLockedText = $"<color={infoHex}>{lockedSkillText}</color>";
        string requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRegiments.text = requirements;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");

        string costColor = skillTree.HasEnoughSkillPoints(skillCost) ? metConditionsHex : notMetConditionsHex;

        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s) </color>");

        foreach (var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? metConditionsHex : notMetConditionsHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName} </color>");
        }

        if (conflictNodes.Length > 0)
        {
            sb.AppendLine("");
            sb.AppendLine($"<color={infoHex}>Locks out: </color>");

            foreach (var node in conflictNodes)
            {
                sb.AppendLine($"<color={infoHex}>- {node.skillData.skillName} </color>");
            }
        }

        return sb.ToString();
    }
}
