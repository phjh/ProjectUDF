using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
	private Transform _playerTransform;
	public float _movementSpeed = 2.5f;

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
		Vector2 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * enemy.ChasingSpeed);
		if (enemy.IsWithStrikingDistance && enemy.canAttack)
		{
			enemy.MoveEnemy(Vector2.zero);
			enemy.StateMachine.ChangeState(enemy.AttackState);
		}
	}

	public override void PhtsicsUpdate()
	{
		base.PhtsicsUpdate();
	}

	public override EnemyState Clone()
	{
		throw new System.NotImplementedException();
	}
}
