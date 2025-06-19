using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    [SerializeField] private string metConditionsHex;
    [SerializeField] private string notMetConditionsHex;
    [SerializeField] private string infoHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is now locked.";
    private Coroutine textEffectCo;

    override protected void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }
        base.ShowToolTip(show, targetRect);
    }
    public void LockedSkillEffect()
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements, .15f, 3));
    }

    public void ShowToolTip(bool show, RectTransform rect, UI_TreeNode node)
    {
        base.ShowToolTip(show, rect);

        if (show == false) return;
        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.description;

        string skillLockedText = GetColoredText(infoHex, lockedSkillText);
        string requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");

        string costColor = skillTree.HasEnoughSkillPoints(skillCost) ? metConditionsHex : notMetConditionsHex;

        sb.AppendLine(GetColoredText(costColor, $"- {skillCost} skill point(s) "));

        foreach (var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? metConditionsHex : notMetConditionsHex;
            sb.AppendLine(GetColoredText(nodeColor, $"- {node.skillData.skillName}"));
        }

        if (conflictNodes.Length > 0)
        {
            sb.AppendLine("");
            sb.AppendLine(GetColoredText(infoHex, "Locks out:"));

            foreach (var node in conflictNodes)
            {
                sb.AppendLine(GetColoredText(infoHex, $"- {node.skillData.skillName}"));
            }
        }

        return sb.ToString();
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (var i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionsHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
            text.text = GetColoredText(infoHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
