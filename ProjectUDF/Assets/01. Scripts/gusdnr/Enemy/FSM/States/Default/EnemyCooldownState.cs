using UnityEngine;

public class EnemyCooldownState : EnemyState
{

	public override void EnterState()
	{
		base.EnterState();
		enemy.IsWithStrikingDistance = false;
		enemy.IsAttackCooldown = true;

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
