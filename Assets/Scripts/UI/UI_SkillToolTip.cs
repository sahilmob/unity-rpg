using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRegiments;

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform rect, Skill_DataSO skillData)
    {
        base.ShowToolTip(show, rect);

        if (show == false) return;
        skillName.text = skillData.skillName;
        skillDescription.text = skillData.description;
        skillRegiments.text = "Requirements: \n"
            + " - " + skillData.cost + " skill points.";
    }
}
