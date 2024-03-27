using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Normal Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState :EnemyState 
{
	public float movementSpeed;

	public NormalChaseState(EnemyMain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
	{
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
