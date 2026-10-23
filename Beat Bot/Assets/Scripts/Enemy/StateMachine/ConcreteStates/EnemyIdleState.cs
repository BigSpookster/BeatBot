using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 targetPos;
    private int nextPos = 0;
    private Vector3 direction;
    private int numPatrolPoints;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

    }

    public override void EnterState()
    {
        base.EnterState();

        numPatrolPoints = enemy.patrolPoints.Length;

        targetPos = GetNextPosition();

    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
            return;

        }

        if ((enemy.transform.position - targetPos).magnitude < 0.5f)
        {
            targetPos = GetNextPosition();

            Debug.Log("New Direction: " + targetPos);

        }

        direction = targetPos - enemy.transform.position;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enemy.MoveEnemy(direction, enemy.MovementSpeed);
        
    }

    private Vector3 GetNextPosition()
    {
        Vector3 tempVector = new Vector3(enemy.patrolPoints[nextPos].position.x, enemy.patrolPoints[nextPos].position.y);

        // Increment nextPos
        nextPos = (nextPos + 1) % numPatrolPoints;

        return tempVector;

    }

}
