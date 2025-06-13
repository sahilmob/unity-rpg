using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = .1f;

    public bool CounterAttackPerformed()
    {
        bool performedCounter = false;

        foreach (Collider2D c in detectedColliders)
        {
            ICounterable counterable = c.GetComponent<ICounterable>();

            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                performedCounter = true;
            }
        }

        return performedCounter;
    }

    public float counterRecoveryDuration
    {

        get => counterRecovery;
    }
}
