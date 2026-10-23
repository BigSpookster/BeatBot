using UnityEngine;
using FMODUnity;

public class SwordEnemyScript : Enemy
{
    private CapsuleCollider2D swordCollider = null;
    private SpriteRenderer swordSprite = null;

    [SerializeField] private float gravityStrength = 1f;

    [SerializeField] private EventReference attackSound;

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
                MusicManager.Instance.PlayOneShotSound(attackSound, this.transform.position);
                EnableSword();

            }

            nextAttack = attackInterval - 1;

        }

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
