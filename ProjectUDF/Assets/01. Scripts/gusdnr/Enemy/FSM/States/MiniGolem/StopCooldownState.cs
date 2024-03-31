using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Stop")]
public class StopCooldownState : EnemyState
{
	public override EnemyState Clone()
	{
		StopCooldownState clone = CloneBase() as StopCooldownState;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.MoveEnemy(Vector2.zero);
		enemy.StartCoroutine(enemy.StartCooldown());
		enemy.EnemySR.color = new Vector4(1, 1, 1, 0.5f);
	}

	public override void ExitState()
	{
		enemy.EnemySR.color = new Vector4(1, 1, 1, 1);
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		enemy.MoveEnemy(Vector2.zero);
		if (enemy.IsAttackCooldown == false)
		{
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}
}
