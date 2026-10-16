using UnityEngine;

public interface IEnemyMovable
{
    Rigidbody2D RB {get; set;}

    bool IsFacingRight {get; set;}

    void MoveEnemy(Vector2 normalizedVelocity, float speed);
    void CheckForLeftOrRightFacing(Vector2 direction);

}
