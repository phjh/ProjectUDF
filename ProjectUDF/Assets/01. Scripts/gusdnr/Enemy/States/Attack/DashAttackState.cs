using System.Collections;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	[Header("Ready Values")]
	public float LockOnTime;
	[Header("Dash Values")]
	public float DashTime;
	public float DashDistance;
	public LayerMask WhatIsEnemy;
	public LayerMask WhatIsObstacle;

	private GridGraph gridGraph;

	private Coroutine LockOnCoroutine;
	private Coroutine AttackCoroutine;
	private Vector2 TargetPos;
	private Vector2 EnemyPos;
	private Vector2 EndPoint;

	public override EnemyState Clone()
	{
		DashAttackState clone = CloneBase() as DashAttackState;
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.LockOnTime = LockOnTime;
		clone.DashTime = DashTime;
		clone.DashDistance = DashDistance;
		clone.WhatIsEnemy = WhatIsEnemy;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		DOTween.Init();
		TargetPos = Vector2.zero;
		EndPoint = Vector2.zero;
		//EnemyPos = enemy.EnemyRB.position;
		enemy.StopAllCoroutines();
		EnemyPos = enemy.EnemyRB.position;

		gridGraph = AstarPath.active.data.gridGraph;

		AttackCoroutine = enemy.StartCoroutine(Dash());

		//공격 순서
		//AttackCoroutine 작동 / LockOnCoroutine 종료 대기 -> LockOnCoroutine 작동 -> 돌진 방향 지정
		//-> LockOnCoroutine 종료 -> 일정 시간 동안 돌진 실행 -> 시간 경과 후 적 이동 속도 0으로 변경해 정지
		//AttackCoroutine 종료 -> CoolDownState로 변경
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.MoveEnemy(Vector2.zero);
		if (AttackCoroutine != null)
		{
			enemy.StopCoroutine(AttackCoroutine);
			AttackCoroutine = null;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if (AttackCoroutine == null)
        {
			Debug.Log("End Attackcoroutine");
			enemy.StopAllCoroutines();
			enemyStateMachine.ChangeState(enemy.CooldownState);
        }
    }

	private IEnumerator Dash()
	{
		enemy.canAttack = false;
		LockOnCoroutine = enemy.StartCoroutine(LockOnTarget());
		
		yield return LockOnCoroutine;

		//적 -> 목표 지점을 향한 정확한 방향 벡터 지정
		Vector2 directionToTarget = (TargetPos - EnemyPos).normalized;
		//적 -> 목표 지점을 향해 Ray를 쏘아 중간에 장애물이 있는지 검사
		RaycastHit2D HitObstacle = Physics2D.Raycast(EnemyPos, directionToTarget, DashDistance, WhatIsObstacle);
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
			EndPoint = EnemyPos + (directionToTarget * DashDistance);
		}
		enemy.CheckForFacing(directionToTarget);
		var dashSeq = DOTween.Sequence();
		
		dashSeq.Append(enemy.transform.DOMove(EndPoint, DashTime).SetEase(Ease.OutCirc));

		dashSeq.Play()
		.OnUpdate(() =>
		{
			PlayerMain player;
			player = Physics2D.OverlapCircle(enemy.EnemyRB.position, 4, WhatIsEnemy).GetComponent<PlayerMain>();
			if (player != null)
			{
				player.GetDamage();
			}
		})
		.OnComplete(() =>
		{
			AttackCoroutine = null;
		});
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		TargetPos = enemy.Target.position;
	}
}
