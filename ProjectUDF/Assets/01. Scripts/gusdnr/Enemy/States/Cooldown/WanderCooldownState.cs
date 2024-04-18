using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Wander")]
public class WanderCooldownState : EnemyState
{
	public float MinDistance = 1f;
	public float MaxDistance = 3f;

	public float MinMoveSpeed = 2f;
	public float MaxMoveSpeed = 5f;
	
	public LayerMask WhatIsObstacle;
	
	private Vector2 targetDirection;
	private Vector3 targetPosition;
	private float moveSpeed;

	public override EnemyState Clone()
	{
		WanderCooldownState clone = (WanderCooldownState)CloneBase();
		clone.MinDistance = MinDistance;
		clone.MaxDistance = MaxDistance;
		clone.MinMoveSpeed = MinMoveSpeed;
		clone.MaxMoveSpeed = MaxMoveSpeed;
		clone.WhatIsObstacle = WhatIsObstacle;
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
		RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, targetDirection, MaxDistance, WhatIsObstacle);
		if (hit.collider != null)
		{
			// 장애물이 감지되면 새로운 방향을 선택하도록 함
			targetDirection = GetNewRandomDirection();
			targetPosition = enemy.transform.position + (Vector3)targetDirection * MaxDistance;
		}
		if (enemy.IsAttackCooldown == false)
		{
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}

	private void SetNewDestination()
	{
		// 랜덤한 방향 설정
		Vector2 randomDirection = Random.insideUnitCircle.normalized;

		// 랜덤한 거리 설정
		float randomDistance = Random.Range(MinDistance, MaxDistance);

		// 랜덤한 위치 설정
		targetPosition = enemy.transform.position + (Vector3)randomDirection * randomDistance;

		// 랜덤한 이동 속도 설정
		moveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
		enemy.MoveEnemy(randomDirection * moveSpeed);
	}

	private Vector2 GetNewRandomDirection()
	{
		// 장애물을 피해서 새로운 랜덤한 방향을 반환하는 메서드
		Vector2 newRandomDirection = Random.insideUnitCircle.normalized;
		// 필요에 따라 추가적인 로직을 구현하여 새로운 방향을 선택할 수 있습니다.
		return newRandomDirection;
	}

}
