using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase EnemyMotionState", menuName = "SO/EnemyMotionState/Chase/Idle")]
public class NonMoveChaseState : EnemyState
{

    public override EnemyState Clone()
    {
        NonMoveChaseState clone = CloneBase() as NonMoveChaseState;
        // 추가적인 초기화가 필요한 경우 여기서 설정
        return clone;
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(enemyStateMachine.CurrentState == this)
            enemy.StateMachine.ChangeState(enemy.AttackState);
    }

}
