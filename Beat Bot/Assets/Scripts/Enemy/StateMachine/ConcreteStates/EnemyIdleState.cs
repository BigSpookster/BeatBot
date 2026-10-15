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

        direction = (targetPos - enemy.transform.position).normalized;

        if ((enemy.transform.position - targetPos).sqrMagnitude < 0.1f)
            targetPos = GetNextPosition();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        enemy.MoveEnemy(direction * enemy.RandomMovementSpeed);
        
    }

    private Vector3 GetNextPosition()
    {
        Vector3 tempVector = new Vector3(enemy.patrolPoints[nextPos].position.x, enemy.patrolPoints[nextPos].position.y);

        // Increment nextPos
        nextPos = (nextPos + 1) % numPatrolPoints;

        return tempVector;

    }

    private Vector3 GetRandomPointInRange()
    {
        return enemy.transform.position + new Vector3(Random.Range(-enemy.RandomMovementRange * 0.5f, enemy.RandomMovementRange * 0.5f), 0);

    }

}
