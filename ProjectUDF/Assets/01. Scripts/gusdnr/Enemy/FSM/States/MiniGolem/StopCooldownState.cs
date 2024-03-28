using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Stop")]
public class StopCooldownState : EnemyState
{
	private Coroutine CooldownCoroutine;

	public override EnemyState Clone()
	{
		StopCooldownState clone = (StopCooldownState)Clone();
		clone.CooldownCoroutine = null;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		CooldownCoroutine = enemy.StartCoroutine(enemy.StartCooldown());
		enemy.ERender.color = new Vector4(1, 1, 1, 0.5f);
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.ERender.color = new Vector4(1, 1, 1, 1);
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if(CooldownCoroutine == null)
		{
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}
}
