using System;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction = NodeDirectionType.None;
    [Range(100, 350)]
    public float length;
    [Range(-50f, 50f)]
    public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{

    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;
    private Image connectionImage;
    private Color originalColor;

    void Awake()
    {
        if (connectionImage != null) originalColor = connectionImage.color;
    }

    void OnValidate()
    {
        if (details.Length <= 0)
        {
            return;
        }

        if (details.Length != connections.Length)
        {
            Debug.Log("Amount of details should be the same as amount of connections - " + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    public void UpdateConnections()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null) continue;

            Image connectionImage = connection.GetConnectionImage();
            Vector2 targetPosition = connection.GetChildConnectionPoint(rect);
            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();
        foreach (var node in details)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateAllConnections();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null) return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image)
    {
        connectionImage = image;
    }

    private void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }
}
