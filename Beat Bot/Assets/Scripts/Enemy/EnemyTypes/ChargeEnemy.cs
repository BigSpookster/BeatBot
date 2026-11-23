using UnityEngine;
using FMODUnity;

public class ChargeEnemy : GroundEnemy
{
    [SerializeField] private float ChargeSpeed = 3f;
    //Animation Stuff
    [SerializeField] private Transform body;
    private SpriteRenderer bodySprite;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        bodySprite = body.GetComponent<SpriteRenderer>();

    }

    protected override void Attack()
    {
        base.Attack();
        bodySprite = body.GetComponent<SpriteRenderer>();

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
                    animator.SetBool("IsCharging", true);
                    charge();
                    break;

                // Finish the attack by freezing again
                case 3:
                    animator.SetBool("IsCharging", false);
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
        bodySprite.flipX = directionOfPlayer.x < 0f;
        MoveEnemy(directionOfPlayer, ChargeSpeed);

    }

    private void UpdateFacing()
    {
        if (playerTransform == null || bodySprite == null)
            return;

        bodySprite.flipX = playerTransform.position.x < transform.position.x;
    }

}
