
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;
    [Header("Battle Details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;
    [Header("Stunned State Details")]
    public float stunnedDuration = 1;
    public bool canBeStunned;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [Header("Movement Details")]
    public float idleTimer = 2f;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection Details")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    public void EnableCounterWindow(bool enabled)
    {
        canBeStunned = enabled;
    }

    public Transform playerReference
    {
        get
        {
            if (player == null)
                player = PlayerDetected().transform;

            return player;
        }
    }

    public void TryEnterBattlerState(Transform player)
    {
        if (stateMachine.currentState == battleState) return;
        if (stateMachine.currentState == attackState) return;

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }

        return hit;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + minRetreatDistance * facingDir, transform.position.y));
    }

    protected override IEnumerator SlowDownCo(float duration, float multiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleMoveSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;
        float speedMultiplier = 1 - multiplier;
        moveSpeed = moveSpeed * speedMultiplier;
        battleMoveSpeed = battleMoveSpeed * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleMoveSpeed;
        anim.speed = originalAnimSpeed;
    }
}
