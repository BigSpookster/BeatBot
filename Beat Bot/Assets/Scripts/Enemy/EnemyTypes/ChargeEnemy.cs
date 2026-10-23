using UnityEngine;
using FMODUnity;

public class ChargeEnemy : Enemy
{
    [SerializeField] private float ChargeSpeed = 3f;
    [SerializeField] private float gravityStrength = 1f;

    [SerializeField] private EventReference attackSound;

    private bool isCharging = false;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void MoveEnemy(Vector2 velocity, float speed)
    {
        base.MoveEnemy(velocity, speed);

        float xDirection = Mathf.Sign(velocity.x);

        // Stop horizontal movement if there is no direction
        if (Mathf.Abs(velocity.x) < 0.01f)
        {
            xDirection = 0f;
        }

        RB.linearVelocity = new Vector2(xDirection * speed, RB.linearVelocity.y + Time.fixedDeltaTime * Physics.gravity.y * gravityStrength);
        CheckForLeftOrRightFacing(velocity);

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
                MusicManager.Instance.PlayOneShotSound(attackSound, this.transform.position);
                MoveEnemy(directionOfPlayer, ChargeSpeed);

            }

            nextAttack = attackInterval - 1;

        }

    }

}
