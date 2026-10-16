using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private bool justEnteredState;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {


    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

    }

    public override void EnterState()
    {
        base.EnterState();
        justEnteredState = true;
        enemy.InAttackState = true;

    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.InAttackState = false;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Change to chase state
        if (!enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
            return;
            
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (justEnteredState)
        {
            // Stop the enemy
            enemy.MoveEnemy(new Vector2 (0, 0), 0);

            justEnteredState = false;

        }
        
    }

    

}
