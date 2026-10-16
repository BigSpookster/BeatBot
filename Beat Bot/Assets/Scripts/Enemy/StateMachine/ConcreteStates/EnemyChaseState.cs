using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform playerTransform;
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
        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
            return;

        }

        // Enter Idle State
        if (!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
            return;

        }

        Vector2 moveDirection = (playerTransform.position - enemy.transform.position).normalized;

        enemy.MoveEnemy(moveDirection, enemy.AggroedMoveSpeed);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

}
