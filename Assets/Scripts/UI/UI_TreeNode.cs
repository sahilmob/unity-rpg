using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool isLocked;
    public bool isUnlocked;
    private Color lastColor;
    private string lockedColorHex = "#909090";
    [SerializeField] private Image skillIcon;

    private bool CanBeUnlocked
    {
        get => (isLocked || isUnlocked) ? false : true;
    }

    void Awake()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
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
        if (!isUnlocked)
            UpdateIconColor(Color.white * 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
