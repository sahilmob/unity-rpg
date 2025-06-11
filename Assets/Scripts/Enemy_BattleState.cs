using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    private float playerDistance
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
            return playerDistance < enemy.attackDistance;
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

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = enemy.PlayerDetection().transform;
    }


    public override void Update()
    {
        base.Update();

        if (withinAttackRange)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * directionToPlayer, rb.linearVelocityY);
        }
    }
}
