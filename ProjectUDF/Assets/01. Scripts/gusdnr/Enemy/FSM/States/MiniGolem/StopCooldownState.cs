using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Stop")]
public class StopCooldownState : EnemyState
{
	private Coroutine CooldownCoroutine;

	public override EnemyState Clone()
	{
		StopCooldownState clone = CloneBase() as StopCooldownState;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		Debug.Log("Enter CooldownState");
		enemy.StopAllCoroutines();
		CooldownCoroutine = enemy.StartCoroutine(enemy.StartCooldown());
		enemy.ERender.color = new Vector4(1, 1, 1, 0.5f);
	}

	public override void ExitState()
	{
		enemy.ERender.color = new Vector4(1, 1, 1, 1);
		base.ExitState();
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
