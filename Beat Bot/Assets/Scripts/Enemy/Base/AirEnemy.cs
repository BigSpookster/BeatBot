using UnityEngine;

public class AirEnemy : Enemy
{
    public override void MoveEnemy(Vector2 velocity, float speed)
    {
        base.MoveEnemy(velocity, speed);

        RB.linearVelocity = velocity.normalized * speed;
        CheckForLeftOrRightFacing(velocity);

    }
}
