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
			Debug.Log("Attack");
		}
	}

	private IEnumerator LockOnAndDash(EnemyBase eb)
	{
		LockOnTarget = false;
		yield return StartCoroutine(LockOn(eb, lockOnDelay));

		if (LockOnTarget)
		{
			yield return StartCoroutine(Dash(eb));

			// ���⿡�� �뽬 �� ó��
			eb.CooldownAttack();
		}
	}

	private IEnumerator LockOn(EnemyBase eb, float delay)
	{
		yield return new WaitForSeconds(delay);
		TargetPos = eb.TargetPos;
		LockOnTarget = true;
	}

	private IEnumerator Dash(EnemyBase eb)
	{
		Vector2 direction = (TargetPos - eb.EnemyPos).normalized; 
		eb.EnemyRB.velocity = direction * dashSpeed; // Rigidbody2D.velocity�� �̿��� �̵�
		yield return new WaitForSeconds(dashDuration); // �뽬 ���� �ð� ���� ���
		eb.EnemyRB.velocity = Vector2.zero; // �뽬 ���� �� �ӵ� �ʱ�ȭ
	}
}
