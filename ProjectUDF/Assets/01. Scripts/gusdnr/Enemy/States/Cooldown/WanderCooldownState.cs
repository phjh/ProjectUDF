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
			// ��ֹ��� �����Ǹ� ���ο� ������ �����ϵ��� ��
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
		// ������ ���� ����
		Vector2 randomDirection = Random.insideUnitCircle.normalized;

		// ������ �Ÿ� ����
		float randomDistance = Random.Range(MinDistance, MaxDistance);

		// ������ ��ġ ����
		targetPosition = enemy.transform.position + (Vector3)randomDirection * randomDistance;

		// ������ �̵� �ӵ� ����
		moveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
		enemy.MoveEnemy(randomDirection * moveSpeed);
	}

	private Vector2 GetNewRandomDirection()
	{
		// ��ֹ��� ���ؼ� ���ο� ������ ������ ��ȯ�ϴ� �޼���
		Vector2 newRandomDirection = Random.insideUnitCircle.normalized;
		// �ʿ信 ���� �߰����� ������ �����Ͽ� ���ο� ������ ������ �� �ֽ��ϴ�.
		return newRandomDirection;
	}

}
