using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isLocked;
    public bool isUnlocked;

    [Header("Skill Details")]
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] public Skill_DataSO skillData;
    [SerializeField] private int skillCost;
    private Color lastColor;
    private string lockedColorHex = "#909090";

    private bool CanBeUnlocked
    {
        get
        {

            if (isLocked || isUnlocked) return false;
            if (!skillTree.HasEnoughSkillPoints(skillData.cost)) return false;

            foreach (var node in neededNodes)
            {
                if (!node.isUnlocked)
                {
                    return false;
                }
            }

            foreach (var node in conflictNodes)
            {
                if (node.isUnlocked)
                {
                    return false;
                }
            }

            return true;
        }
    }

    void OnValidate()
    {
        if (!skillData) return;
        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.skillName;
    }

    void Awake()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictNodes();
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null) return;
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked)
        {
            Unlock();
        }
        else
        {
            Debug.Log("Cannot be unlocked");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);
        if (!isUnlocked)
            UpdateIconColor(Color.white * 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);
        if (!isUnlocked)
        {
            UpdateIconColor(lastColor);
        }
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
        }
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }
}
