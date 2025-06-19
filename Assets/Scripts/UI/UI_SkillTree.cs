using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    public Player_SkillManager skillManager { get; private set; }
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;

    void Awake()
    {
        skillManager = FindFirstObjectByType<Player_SkillManager>();
    }

    public bool HasEnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
    }

    public void AddSkillPoints(int points) => skillPoints += points;

    [ContextMenu("Reset skill tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] nodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var n in nodes)
        {
            n.Refund();
        }
    }

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update all connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
