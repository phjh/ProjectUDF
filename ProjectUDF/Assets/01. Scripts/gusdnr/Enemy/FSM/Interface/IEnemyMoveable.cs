using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D EnemyRB { get; set; }
    bool IsFacingRight { get; set; }

    void MoveEnemy(Vector2 moveVelocity);
    void CheckForFacing(Vector2 velocity);
}
