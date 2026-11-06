using UnityEngine;

public class EnemyAttackState : EnemyState
{
    Vector2 moveDirection;
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
        
    }

    

}
