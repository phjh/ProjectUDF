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
		// 랜덤한 방향 설정
		Vector2 randomDirection = Random.insideUnitCircle.normalized;

		// 랜덤한 거리 설정
		float randomDistance = Random.Range(MinDistance, MaxDistance);

		// 랜덤한 위치 설정
		targetPosition = enemy.transform.position + (Vector3)randomDirection * randomDistance;

		// 랜덤한 이동 속도 설정
		moveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);

	}


}
