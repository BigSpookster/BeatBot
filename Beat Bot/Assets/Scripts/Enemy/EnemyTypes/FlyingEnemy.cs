using FMODUnity;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] private float ChargeSpeed = 3f;

    [SerializeField] private EventReference windupSound;
    [SerializeField] private EventReference attackSound;
    private bool isCharging = false;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void MoveEnemy(Vector2 normalizedVelocity, float speed)
    {
        base.MoveEnemy(normalizedVelocity, speed);

        RB.linearVelocity = normalizedVelocity * speed;
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
                MusicManager.instance.PlayOneShotSound(attackSound, this.transform.position);
                MoveEnemy(directionOfPlayer, ChargeSpeed);

            }

            nextAttack = attackInterval - 1;

        }

    }
    
}
