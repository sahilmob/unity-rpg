using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private string skillName;
    [SerializeField] private Skill_DataSO skillData;
    private UI ui;
    private RectTransform rect;

    public bool isLocked;
    public bool isUnlocked;
    private Color lastColor;
    private string lockedColorHex = "#909090";
    [SerializeField] private Image skillIcon;

    private bool CanBeUnlocked
    {
        get => (isLocked || isUnlocked) ? false : true;
    }

    void OnValidate()
    {
        if (!skillData) return;
        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillData.skillName;
    }

    void Awake()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, skillData);
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
        Debug.Log("Hide skill tooltip");
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }
}
