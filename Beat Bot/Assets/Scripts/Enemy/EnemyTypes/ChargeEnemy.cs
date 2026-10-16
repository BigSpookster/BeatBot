using UnityEngine;

public class ChargeEnemy : Enemy
{
    [SerializeField] private float ChargeSpeed = 3f;
    private bool isCharging = false;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void MoveEnemy(Vector2 normalizedVelocity, float speed)
    {
        base.MoveEnemy(normalizedVelocity, speed);

        int makeEqualOne = Mathf.RoundToInt(normalizedVelocity.x);

        RB.linearVelocity = new Vector2(makeEqualOne * speed, RB.linearVelocity.y);
        CheckForLeftOrRightFacing(normalizedVelocity);

    }

    protected override void Attack()
    {
        base.Attack();

        // If we have not reached the next attack, decrement
        if (nextAttack > 0)
        {
            nextAttack--;

            if (isCharging)
            {
                // Go back to chase state after charging
                StateMachine.ChangeState(ChaseState);

                isCharging = false;
                
            }

        }

        // Otherwise, toggle the sword and reset nextAttack
        else
        {
            // Only perform the attack if enemy is in attack state!
            if (InAttackState)
            {
                Vector2 directionOfPlayer = (playerTransform.position - this.transform.position).normalized;
                isCharging = true;
                MoveEnemy(directionOfPlayer, ChargeSpeed);

            }

            nextAttack = attackInterval - 1;

        }

    }

}
