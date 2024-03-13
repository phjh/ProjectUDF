using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashAttack : AtkPatternMono
{
	[SerializeField] private float lockOnDelay = 1.5f;
	[SerializeField] private float dashSpeed = 8f; // 돌진 속도
	[SerializeField] private float dashDuration = 1.5f;

	private Vector2 TargetPos;
	private bool LockOnTarget;

	public override void DoingAttack(EnemyBase eb)
	{
		
		if (eb != null && eb.Target != null)
		{
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
		Vector2 direction = (TargetPos - eb.EnemyPos).normalized;
		Vector2 validDestination = FindNearestPosition(eb.EnemyPos + direction * 10f);
		float elapsedTime = 0f;

		while (elapsedTime < dashDuration)
		{
			// 부드럽게 이동하는 대신, 더 자주 업데이트하여 더 빠르게 이동
			eb.aiPath.destination = Vector2.Lerp(eb.EnemyPos, validDestination, elapsedTime / dashDuration);
			yield return null;
			elapsedTime += Time.deltaTime * dashSpeed; // 대쉬 속도 적용
		}

		eb.aiPath.destination = eb.EnemyPos; //목적지를 현재 자신의 위치로 지정해 정지
		Debug.Log("End Dash");
	}

	private Vector2 FindNearestPosition(Vector2 targetPosition)
	{
		//A* graph data를 이용해 입력 받은 좌표 주위의 Bake된 이동할 수 있는 좌표값을 반환한다.
		NNInfo nearestNodeInfo = AstarPath.active.GetNearest(targetPosition, NNConstraint.Default);
		return nearestNodeInfo.position;
	}
}
