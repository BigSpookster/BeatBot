using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform playerTransform;

    private Vector2 moveDirection;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerTransform = player.transform;

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

    }

    public override void EnterState()
    {
        base.EnterState();

    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Enter Idle State
        if (!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
            return;

        }

        moveDirection = playerTransform.position - enemy.transform.position;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        // Only handle movement if enemy is not attacking.
        if (!enemy.isAttacking)
            enemy.MoveEnemy(moveDirection, enemy.AggroedMoveSpeed);
        
    }

}
