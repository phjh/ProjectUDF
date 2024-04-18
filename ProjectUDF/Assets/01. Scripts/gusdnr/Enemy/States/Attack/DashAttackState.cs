using System.Collections;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	[Header("Ready Values")]
	public float LockOnTime;
	[Header("Dash Values")]
	public float DashTime;
	public float DashDistance;
	public LayerMask WhatIsObstacle;

	GridGraph gridGraph;

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
            enemy.StateMachine.ChangeState(enemy.CooldownState);
        }
    }

	private IEnumerator Dash()
	{
		enemy.canAttack = false;
		LockOnCoroutine = enemy.StartCoroutine(LockOnTarget());
		
		yield return LockOnCoroutine;

		Vector2 directionToTarget = (TargetPos - EnemyPos).normalized;

		RaycastHit2D HitObstacle = Physics2D.Raycast(EnemyPos, directionToTarget, DashDistance, WhatIsObstacle);
		Debug.Log($"Is Checking Obstacle : {(bool)HitObstacle}");
		if (HitObstacle)
		{
			Vector2 HitPos = HitObstacle.point;
			HitPos.x = (HitPos.x > EnemyPos.x) ? HitPos.x - 0.5f : HitPos.x + 0.5f;
			HitPos.y = (HitPos.y > EnemyPos.y) ? HitPos.y - 0.5f : HitPos.y + 0.5f;

			// 주변 노드 찾기
			NNInfoInternal nearestNodeInfo = gridGraph.GetNearest(HitPos, NNConstraint.None);

			GraphNode nearestNode = nearestNodeInfo.node;
			Vector3 worldPosition = (Vector3)nearestNode.position;
			
			EndPoint = worldPosition;
			Debug.DrawRay(EnemyPos, EndPoint, Color.blue, DashDistance);
			yield return new WaitForSeconds(1);
		}
		else if (!HitObstacle)
		{
			EndPoint = EnemyPos + (directionToTarget * DashDistance);
			Debug.Log($"End Pos.normal : [{directionToTarget.x}] [{directionToTarget.y}]");
		}
		Debug.Log($"End Point : [X: {EndPoint.x}] [Y: {EndPoint.y}]");
		enemy.CheckForFacing(directionToTarget);
		yield return new WaitForSeconds(1);
		Debug.DrawRay(EnemyPos, EndPoint, Color.magenta, DashDistance);
		var dashSeq = DOTween.Sequence();

		dashSeq.Append(enemy.transform.DOMove(EndPoint, DashTime).SetEase(Ease.OutCirc));

		dashSeq.Play().OnComplete(() =>
		{
			AttackCoroutine = null;
		});
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		TargetPos = enemy.Target.position;
		Debug.Log($"Input : Target Pos : [X: {TargetPos.x}] [Y: {TargetPos.y}]");

		//Debug.DrawRay(EnemyPos, TargetPos, Color.green, DashDistance);
	}
}
