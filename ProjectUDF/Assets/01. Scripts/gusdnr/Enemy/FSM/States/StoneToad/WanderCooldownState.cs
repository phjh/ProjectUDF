using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Wander")]
public class WanderCooldownState : EnemyState
{
	public float MinDistance = 1f;
	public float MaxDistance = 3f;

	public float MinMoveSpeed = 2f;
	public float MaxMoveSpeed = 5f;

	private Vector3 targetPosition;
	private float moveSpeed;

	public override EnemyState Clone()
	{
		WanderCooldownState clone = (WanderCooldownState)CloneBase();
		clone.MinDistance = MinDistance;
		clone.MaxDistance = MaxDistance;
		clone.MinMoveSpeed = MinMoveSpeed;
		clone.MaxMoveSpeed = MaxMoveSpeed;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
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
		if (Vector3.Distance(enemy.transform.position, targetPosition) <= MinDistance)
		{
			SetNewDestination();
		}

	}

	void SetNewDestination()
	{
		// ������ ���� ����
		Vector2 randomDirection = Random.insideUnitCircle.normalized;

		// ������ �Ÿ� ����
		float randomDistance = Random.Range(MinDistance, MaxDistance);

		// ������ ��ġ ����
		targetPosition = enemy.transform.position + (Vector3)randomDirection * randomDistance;

		// ������ �̵� �ӵ� ����
		moveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);

	}


}
