using DG.Tweening;
using Pathfinding;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Wander")]
public class WanderCooldownState : EnemyState
{
	public float MinDistance = 1f;
	public float MaxDistance = 3f;
	public float MoveTime = 5f;

	public LayerMask WhatIsObstacle;

	private GridGraph gridGraph;

	private Vector2 EnemyPos;
	private Vector2 EndPoint;
	private Vector2 targetDirection;

	public override EnemyState Clone()
	{
		WanderCooldownState clone = (WanderCooldownState)CloneBase();
		clone.MinDistance = MinDistance;
		clone.MaxDistance = MaxDistance;
		clone.MoveTime = MoveTime;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		DOTween.Init();
		enemy.MoveEnemy(Vector2.zero);

		gridGraph = AstarPath.active.data.gridGraph;
		SetNewDestination();
		enemy.StartCoroutine(enemy.StartCooldown());
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.MoveEnemy(Vector2.zero);
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		EnemyPos = enemy.EnemyRB.position;
		if (enemy.IsAttackCooldown == false)
		{
			enemy.DOKill();
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}

	private void SetNewDestination()
	{
		// 랜덤한 방향 설정
		Vector2 randomDirection = Random.insideUnitCircle.normalized;
		Vector2 moveDIr = (randomDirection - EnemyPos).normalized;
		float distance = Random.Range(MinDistance, MaxDistance);
		// 랜덤한 거리 설정
		RaycastHit2D HitObstacle = Physics2D.Raycast(enemy.EnemyRB.position, moveDIr, distance, WhatIsObstacle);
		if (HitObstacle)
		{
			//장애물 위치로 지정
			Vector2 HitPos = HitObstacle.point;
			//콜라이더 크기만큼 빼주어 A* Graph 이탈 방지
			HitPos.x = (HitPos.x > EnemyPos.x) ? HitPos.x - 0.5f : HitPos.x + 0.5f;
			HitPos.y = (HitPos.y > EnemyPos.y) ? HitPos.y - 0.5f : HitPos.y + 0.5f;

			//탐지 위치 주변 노드 찾기
			NNInfoInternal nearestNodeInfo = gridGraph.GetNearest(HitPos, NNConstraint.None);
			//탐지된 노드 지정
			GraphNode nearestNode = nearestNodeInfo.node;
			//노드 위치 Unity World Position화
			Vector3 worldPosition = (Vector3)nearestNode.position;
			//도착 지점 설정
			EndPoint = worldPosition;
		}
		else if (!HitObstacle)
		{
			//도착 지점 현재 위치에서 방향 벡터와 돌진 거리만큼 곱한 값으로 지정
			EndPoint = EnemyPos + (moveDIr * distance);
		}
		enemy.CheckForFacing(moveDIr);

		var wanderSeq = DOTween.Sequence();

		wanderSeq.Append(enemy.transform.DOMove(EndPoint, MoveTime).SetEase(Ease.InQuad));

		wanderSeq.Play().OnComplete(() =>
		{
			if (enemy.IsAttackCooldown == true)
			{
				SetNewDestination();
			}
			if (enemy.IsAttackCooldown == false)
			{
				enemy.StateMachine.ChangeState(enemy.ChaseState);
			}
		});
	}
}
