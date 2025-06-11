using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    private float distanceToPlayer
    {
        get
        {
            if (player == null)
            {
                return float.MaxValue;
            }

            return Mathf.Abs(player.position.x - enemy.transform.position.x);
        }
    }

    private bool withinAttackRange
    {
        get
        {
            return distanceToPlayer < enemy.attackDistance;
        }
    }

    private int directionToPlayer
    {
        get
        {
            if (player == null) return 0;
            return enemy.transform.position.x > player.position.x ? -1 : 1;
        }
    }

    public bool shouldRetreat
    {
        get
        {
            return distanceToPlayer < enemy.minRetreatDistance ? true : false;
        }
    }

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = enemy.PlayerDetected().transform;

        if (shouldRetreat)
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -directionToPlayer, enemy.retreatVelocity.y);
            enemy.HandleFlip(directionToPlayer);
        }
    }


    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            lastTimeWasInBattle = Time.time;
        }

        if (Time.time > lastTimeWasInBattle + 5)
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if (withinAttackRange && enemy.PlayerDetected())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * directionToPlayer, rb.linearVelocityY);
        }
    }
}
