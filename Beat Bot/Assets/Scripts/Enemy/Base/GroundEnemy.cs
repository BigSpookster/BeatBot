using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] private float gravityStrength = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float hitStunDuration = 0.2f;

    private float hitStunTimer = 0f;

    protected override void Awake()
    {
        base.Awake();

        isGroundEnemy = true;

    }

    public override void MoveEnemy(Vector2 velocity, float speed)
    {
        base.MoveEnemy(velocity, speed);

        if (hitStunTimer > 0f)
        {
            hitStunTimer -= Time.fixedDeltaTime;

            if (hitStunTimer <= 0f)
                IsInHitStun = false;

            return;
        }

        float xDirection = Mathf.Sign(velocity.x);

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

    public void TakeHit(Vector2 hitboxPosition)
    {
        Vector2 knockbackDirection = ((Vector2)transform.position - hitboxPosition).normalized;

        hitStunTimer = hitStunDuration;
        IsInHitStun = true;
        RB.linearVelocity = Vector2.zero;
        RB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
