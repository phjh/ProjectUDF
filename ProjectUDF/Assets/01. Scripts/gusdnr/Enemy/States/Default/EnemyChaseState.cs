using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
	public EnemyChaseState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
	}

	public override void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType)
	{
		base.AnimationTriggerEvent(triggerType);
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
	}

	public override void PhtsicsUpdate()
	{
		base.PhtsicsUpdate();
	}
}
