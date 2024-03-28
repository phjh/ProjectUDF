using UnityEngine;

public class EnemyCooldownState : EnemyState
{

	public override EnemyState Clone()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		base.EnterState();

		enemy.StartCoroutine(enemy.StartAttackCooldown());
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		// Cooldown logic here
		if (enemy.IsAttackCooldown == false)
		{
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}
}
