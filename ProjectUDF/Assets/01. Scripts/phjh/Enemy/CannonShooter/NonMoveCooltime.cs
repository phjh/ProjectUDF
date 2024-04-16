using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cannon Cooldown State", menuName = "SO/State/Cooldown/ThrowCooltime")]
public class NonMoveCooltime : EnemyState
{
    public override EnemyState Clone()
    {
        NonMoveCooltime clone = CloneBase() as NonMoveCooltime;
        return clone;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.StartCoroutine(enemy.StartCooldown());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.IsAttackCooldown == false)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }

}
