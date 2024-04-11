using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.movementSpeed = movementSpeed;
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
		//플레이어가 시야 내에 있을 때
		if (enemy.IsWithStrikingDistance)
		{
			Debug.Log("Enemy In AttackRange");
			//공격 가능한 상태일 때
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				Debug.Log("Enemy Do Attack");
				// 공격 가능한 상태로 전환
				enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
			}
			else
				HandleObstacleDetection(); // 장애물 감지 및 이동 처리
		}
		else
		{
			HandleObstacleDetection();
		}
	}

	private void HandleObstacleDetection()
	{

		Debug.Log("Enemy Do Search");
		// 장애물 감지
		bool obstacleDetected = !enemy.UpdateFOV();

		// 장애물이 감지된 경우
		if (obstacleDetected)
			MoveRandomly(); // 임의의 방향으로 회전하여 이동
		else
			MoveTowardsPlayer(); // 장애물이 없으면 플레이어 방향으로 직진
	}

	private void MoveRandomly()
	{

		Debug.Log("Enter Move Random");

		Vector2 closestDirection = (enemy.Target.position - enemy.transform.position).normalized; ; 
		float closestDistance = Vector2.Distance(enemy.transform.position, enemy.Target.position);

		// 적의 현재 위치에서 타겟까지의 방향 벡터 계산

		// 타겟을 향한 방향 벡터를 기준으로, 시계방향과 반시계방향으로 90도 회전한 벡터를 계산
		Vector2 toTargetDirection = (enemy.Target.position - enemy.transform.position).normalized; ;
		Vector2 rightAngleDirection = Quaternion.Euler(0, 0, -90) * toTargetDirection;
		Vector2 leftAngleDirection = Quaternion.Euler(0, 0, 90) * toTargetDirection;

		// 주어진 각도 범위 내에서 반복하여 타겟과 더 가까운 방향을 찾습니다.
		for (int i = 0; i < 9; i++) // 40도 간격으로 9번 반복합니다.
		{
			// 현재 반복 인덱스에 따라 시계방향 또는 반시계방향으로 회전한 방향 계산
			Vector2 rotatedDirection = (i % 2 == 0) ? Quaternion.Euler(0, 0, i * 20) * rightAngleDirection : Quaternion.Euler(0, 0, i * 20) * leftAngleDirection;
			Debug.Log("Searching Move Direction");
			// 회전한 방향으로 장애물이 있는지 검사
			RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, rotatedDirection, 10f, enemy.WhatIsObstacle);
			if (hit.collider == null)
			{
				Debug.Log("No Obstacle");
				// 장애물이 없는 경우, 타겟과의 거리를 계산하여 더 가까운 방향을 선택합니다.
				float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.Target.position);
				if (distanceToTarget < closestDistance)
				{
					closestDirection = rotatedDirection;
					closestDistance = distanceToTarget;
				}
			}
			else if (hit.collider != null)
			{
				Debug.Log("Searching Obstacle");
				closestDirection = rotatedDirection;
				break;
			}
		}
		// 계산된 가장 가까운 방향으로 이동합니다.
		Debug.Log($"Set Random Direction [{closestDirection.x} : {closestDirection.y}]");
		enemy.MoveEnemy(closestDirection * movementSpeed);
	}

	private void MoveTowardsPlayer()
	{
		Debug.Log("Enter Move To Player");

		// 플레이어 방향으로 직진
		Vector2 moveDirection = (enemy.Target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);
	}


}
