using UnityEngine;

[CreateAssetMenu(fileName = "SmallGolem Cooldown EnemyMotionState", menuName = "SO/EnemyMotionState/Cooldown/Stop")] 
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
		Debug.Log("Enter Cooldown EnemyMotionState");
		enemy.MoveEnemy(Vector2.zero);
		enemy.StartCoroutine(enemy.StartCooldown());
	}

	public override void ExitState()
	{
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
