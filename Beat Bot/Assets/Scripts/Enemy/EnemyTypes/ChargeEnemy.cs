using UnityEngine;
using FMODUnity;

public class ChargeEnemy : GroundEnemy
{
    [SerializeField] private float ChargeSpeed = 3f;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Attack()
    {
        base.Attack();

        if (isAttacking)
        {
            switch(attackStage)
            {
                case 0:
                    MusicManager.Instance.PlayOneShotSound(attackWindup, this.transform.position);
                    FreezeInPlace();
                    break;

                // Skip 1 beat, then initiate the attack
                case 2:
                    MusicManager.Instance.PlayOneShotSound(attackSound, this.transform.position);
                    charge();
                    break;

                // Finish the attack by freezing again
                case 3:
                    FreezeInPlace();
                    break;

                // Done attacking
                case 4:
                    isAttacking = false;
                    attackStage = 0;
                    break;

            }
            
            // If the attack is over, set the next attack
            if (!isAttacking)
                nextAttack = attackInterval - 1;
                

            else
                attackStage++;

        }

    }

    private void charge()
    {
        Vector2 directionOfPlayer = (playerTransform.position - this.transform.position).normalized;
        MoveEnemy(directionOfPlayer, ChargeSpeed);

    }

}
