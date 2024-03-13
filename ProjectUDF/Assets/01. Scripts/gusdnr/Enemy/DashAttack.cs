using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Debug.Log("Searching...");
		yield return new WaitForSeconds(delay);
		TargetPos = eb.TargetPos;
		LockOnTarget = true;
		Debug.Log($"Lock On Target: {LockOnTarget}");
	}

	private IEnumerator Dash(EnemyBase eb)
	{
		Debug.Log("Start Dash");
		Vector2 direction = (TargetPos - eb.EnemyPos).normalized;
		eb.aiPath.destination = TargetPos; // A*에 목적지 설정
		yield return new WaitForSeconds(dashDuration); // 일정 시간 동안 대쉬 진행
		eb.aiPath.maxSpeed = 0f; //속도를 0으로 해 정지
		Debug.Log("End Dash");
	}
}
