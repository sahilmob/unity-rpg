using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childConnectionPoint;

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Image GetConnectionImage()
    {
        return connectionLength.GetComponent<Image>();
    }

    public Vector2 GetChildConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            childConnectionPoint.position,
            null,
            out var localPosition
        );

        return localPosition;
    }

    public float GetDirectionAngle(NodeDirectionType type)
    {
        return type switch
        {
            NodeDirectionType.UpLeft => 135f,
            NodeDirectionType.Up => 90f,
            NodeDirectionType.UpRight => 45f,
            NodeDirectionType.Left => 180f,
            NodeDirectionType.Right => 0f,
            NodeDirectionType.DownLeft => -135,
            NodeDirectionType.Down => -90f,
            NodeDirectionType.DownRight => -45f,
            _ => 0f,
        };
    }
}


public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}
