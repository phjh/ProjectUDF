using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashAttack : AtkPatternMono
{
	[SerializeField] private float lockOnDelay;
	[SerializeField] private float dashSpeed; // 돌진 속도
	[SerializeField] private float dashDuration;

	private Vector2 TargetPos;
	private bool LockOnTarget;

	public override void DoingAttack(EnemyBase eb)
	{
		if (eb != null && eb.target != null)
		{
			eb.aiPath.destination = eb.EnemyPos;
			StartCoroutine(LockOnAndDash(eb));
		}
	}

	private IEnumerator LockOnAndDash(EnemyBase eb)
	{
		eb.aiPath.isStopped = false;
		LockOnTarget = false;
		yield return StartCoroutine(LockOn(eb, lockOnDelay));

		if (LockOnTarget)
		{
			yield return StartCoroutine(Dash(eb));

			// 여기에서 대쉬 후 처리
			eb.isAttacking = false;
			eb.StartCoroutine("CooldownAttack");
		}
	}

	private IEnumerator LockOn(EnemyBase eb, float delay)
	{
		Vector2 lastKnownPosition = eb.TargetPos; // 초기값 설정

		Debug.Log("Searching...");

		// 주기적으로 적의 위치를 업데이트
		while (delay > 0f)
		{
			lastKnownPosition = eb.TargetPos;
			yield return new WaitForSeconds(0.1f);
			delay -= 0.1f;
		}

		// 타이머가 끝나면 마지막으로 받은 위치를 TargetPos로 설정
		TargetPos = lastKnownPosition;
		LockOnTarget = true;
		Debug.Log($"Lock On Target: {LockOnTarget}");
	}

	private IEnumerator Dash(EnemyBase eb)
	{
		Debug.Log("Start Dash");
		eb.aiPath.maxSpeed = dashSpeed;
		Vector2 direction = (TargetPos - eb.EnemyPos).normalized;
		float elapsedTime = 0f;
		float dashTime = dashDuration / 2f;

		while (elapsedTime < dashDuration)
		{
			eb.aiPath.destination = Vector2.Lerp(eb.EnemyPos, FindNearestPosition(eb.EnemyPos + direction * 2f), elapsedTime / dashTime) + direction * dashSpeed * Time.deltaTime;
			yield return null;
			elapsedTime += Time.deltaTime;
		}

		eb.aiPath.destination = eb.EnemyPos; // 목적지를 현재 자신의 위치로 지정해 정지
		Debug.Log("End Dash");
	}


	private Vector2 FindNearestPosition(Vector2 targetPosition)
	{
		//A* graph data를 이용해 입력 받은 좌표 주위의 Bake된 이동할 수 있는 좌표값을 반환한다.
		NNInfo nearestNodeInfo = AstarPath.active.GetNearest(targetPosition, NNConstraint.Default);
		return nearestNodeInfo.position;
	}
}
