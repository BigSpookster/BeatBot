using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] private float gravityStrength = 1f;

    protected override void Awake()
    {
        base.Awake();

        isGroundEnemy = true;

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

    protected void FreezeInPlace()
    {
        RB.linearVelocity = new Vector2(0, RB.linearVelocity.y + Time.fixedDeltaTime * Physics.gravity.y * gravityStrength);

    }


}
