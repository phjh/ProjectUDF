using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : AtkPatternMono
{
	[SerializeField] private float lockOnDelay = 1.5f;
	[SerializeField] private float dashSpeed = 8f; // ���� �ӵ�
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

			// ���⿡�� �뽬 �� ó��
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
		eb.aiPath.destination = TargetPos; // A*�� ������ ����
		yield return new WaitForSeconds(dashDuration); // ���� �ð� ���� �뽬 ����
		eb.aiPath.maxSpeed = 0f; //�ӵ��� 0���� �� ����
		Debug.Log("End Dash");
	}
}
