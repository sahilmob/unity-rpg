using System;
using UnityEngine;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction = NodeDirectionType.None;
    [Range(100, 350)]
    public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{

    private RectTransform rect;
    [SerializeField] private UI_TreeConnectDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    void OnValidate()
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        if (details.Length != connections.Length)
        {
            Debug.Log("Amount of details should be the same as amount of connections - " + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            connection.DirectConnection(detail.direction, detail.length);
            Vector2 targetPosition = connection.GetChildConnectionPoint(rect);
            detail?.childNode?.SetPosition(targetPosition);
        }
    }

    private void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }
}
