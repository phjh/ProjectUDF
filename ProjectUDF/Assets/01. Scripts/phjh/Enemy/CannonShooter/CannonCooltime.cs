using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cannon Cooldown State", menuName = "SO/State/Cooldown/ThrowCooltime")]
public class CannonCooltime : EnemyState
{
    public override EnemyState Clone()
    {
        CannonCooltime clone = CloneBase() as CannonCooltime;
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
