using UnityEngine;

public class SwordEnemyScript : Enemy
{
    private CapsuleCollider2D swordCollider = null;
    private SpriteRenderer swordSprite = null;

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

    }

    

    protected override void Attack()
    {
        // If we have not reached the next attack, decrement
        if (nextAttack > 0)
        {
            nextAttack--;

            // If the sword is enabled, we want to disable it
            if (swordCollider.enabled)
                DisableSword();

        }

        // Otherwise, toggle the sword and reset nextAttack
        else
        {
            // Only perform the attack if enemy is in attack state!
            if (InAttackState)
            {
                Vector2 directionOfPlayer = (playerTransform.position - this.transform.position).normalized;
                CheckForLeftOrRightFacing(directionOfPlayer);
                EnableSword();

            }

            nextAttack = attackInterval - 1;

        }

    }

    public override void MoveEnemy(Vector2 normalizedVelocity, float speed)
    {
        base.MoveEnemy(normalizedVelocity, speed);

        int makeEqualOne = Mathf.RoundToInt(normalizedVelocity.x);

        RB.linearVelocity = new Vector2(makeEqualOne * speed, RB.linearVelocity.y);
        CheckForLeftOrRightFacing(normalizedVelocity);

    }

    // Enables the sword's sprite and collider
    private void EnableSword()
    {
        swordCollider.enabled = true;
        swordSprite.enabled = true;

    }

    // Disables the sword's sprite and collider
    private void DisableSword()
    {
        swordCollider.enabled = false;
        swordSprite.enabled = false;

    }

}
