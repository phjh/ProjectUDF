using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;

	public override EnemyState Clone()
	{
		NormalChaseState clone = (NormalChaseState)CloneBase();
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.movementSpeed = this.movementSpeed;
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
		Vector2 moveDirection = (enemy.Target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);

		if (enemy.IsWithStrikingDistance)
		{
			enemy.MoveEnemy(Vector2.zero);
			enemy.StateMachine.ChangeState(enemy.AttackState);
		}
	}

	public override void PhtsicsUpdate()
	{
		base.PhtsicsUpdate();
	}
}
