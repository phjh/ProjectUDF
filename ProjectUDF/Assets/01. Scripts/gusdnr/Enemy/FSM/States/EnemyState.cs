using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyMain enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyMain enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    
    public virtual void FrameUpdate() { }
    public virtual void PhtsicsUpdate() { }

    public virtual void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType) { }
}
