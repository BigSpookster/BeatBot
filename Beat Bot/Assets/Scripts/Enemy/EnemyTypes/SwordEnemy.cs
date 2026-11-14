using UnityEngine;
using FMODUnity;

public class SwordEnemy : GroundEnemy
{
    private CapsuleCollider2D swordCollider = null;
    private SpriteRenderer swordSprite = null;

    private CapsuleCollider2D upAttackCollider = null;
    private SpriteRenderer upAttackSprite = null;

    [SerializeField] private float minAngle = 60f;

    protected override void Awake()
    {
        base.Awake();
        
        // Finding the correct child
        Transform checkChildTranform = transform.Find("Sword");

        // If it exists, get the sword's collider and sprite
        if (checkChildTranform != null)
        {
            swordCollider = checkChildTranform.GetComponent<CapsuleCollider2D>();
            swordSprite = checkChildTranform.GetComponent<SpriteRenderer>();

        }

        // Otherwise, there might be an error
        else
            Debug.Log("Error: sword child was not found...");

        // Finding the UpAttack child
        checkChildTranform = transform.Find("UpAttack");

        // If it exists, get the sword's collider and sprite
        if (checkChildTranform != null)
        {
            upAttackCollider = checkChildTranform.GetComponent<CapsuleCollider2D>();
            upAttackSprite = checkChildTranform.GetComponent<SpriteRenderer>();

        }

        // Otherwise, there might be an error
        else
            Debug.Log("Error: UpAttack child was not found...");

    }

    

    protected override void Attack()
    {
        base.Attack();

        if (isAttacking)
        {
            switch (attackStage)
            {
                // Get ready to swing!
                case 0:
                    MusicManager.Instance.PlayOneShotSound(attackWindup, this.transform.position);
                    FreezeInPlace();
                    break;

                // Skip 1 beat, then initiate the attack
                case 2:
                    MusicManager.Instance.PlayOneShotSound(attackSound, this.transform.position);
                    InitiateSwing();
                    break;

                // Finish the attack
                case 3:
                    DisableSword();
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

    private void InitiateSwing()
    {
        // Make sure the attack faces the player
        Vector2 directionOfPlayer = (playerTransform.position - this.transform.position).normalized;

        // Check if the player is above the sword enemy
        float angle = Mathf.Atan2(directionOfPlayer.y, directionOfPlayer.x) * Mathf.Rad2Deg;
        if (angle >= minAngle && angle <= 180 - minAngle)
            EnableUpAttack();

        else
        {
            CheckForLeftOrRightFacing(directionOfPlayer);
            
            EnableSword();

        }
        
    }

    private void EnableUpAttack()
    {
        upAttackCollider.enabled = true;
        upAttackSprite.enabled = true;

    }

    // Enables the sword's sprite and collider
    private void EnableSword()
    {
        swordCollider.enabled = true;
        swordSprite.enabled = true;

    }

    // Disables the sword's sprite and collider, even for UpAttacks
    private void DisableSword()
    {
        swordCollider.enabled = false;
        swordSprite.enabled = false;

        upAttackCollider.enabled = false;
        upAttackSprite.enabled = false;

    }

}
