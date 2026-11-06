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

        // Enter Attack State
        if (enemy.isAttacking)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);

        }

        // Enter Idle State
        if (!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
            return;

        }

        // Make enemy stop moving towards player when they get close enough to strike
        if (enemy.IsWithinStrikingDistance)
            moveDirection = new Vector2(0, 0);

        else
            moveDirection = playerTransform.position - enemy.transform.position;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.MoveEnemy(moveDirection, enemy.AggroedMoveSpeed);
        
    }

}
