using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashAttack : AtkPatternMono
{
	[SerializeField] private float lockOnDelay;
	[SerializeField] private float dashSpeed; // ���� �ӵ�
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

			// ���⿡�� �뽬 �� ó��
			eb.isAttacking = false;
			eb.StartCoroutine("CooldownAttack");
		}
	}

	private IEnumerator LockOn(EnemyBase eb, float delay)
	{
		Vector2 lastKnownPosition = eb.TargetPos; // �ʱⰪ ����

		Debug.Log("Searching...");

		// �ֱ������� ���� ��ġ�� ������Ʈ
		while (delay > 0f)
		{
			lastKnownPosition = eb.TargetPos;
			yield return new WaitForSeconds(0.1f);
			delay -= 0.1f;
		}

		// Ÿ�̸Ӱ� ������ ���������� ���� ��ġ�� TargetPos�� ����
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

		eb.aiPath.destination = eb.EnemyPos; // �������� ���� �ڽ��� ��ġ�� ������ ����
		Debug.Log("End Dash");
	}


	private Vector2 FindNearestPosition(Vector2 targetPosition)
	{
		//A* graph data�� �̿��� �Է� ���� ��ǥ ������ Bake�� �̵��� �� �ִ� ��ǥ���� ��ȯ�Ѵ�.
		NNInfo nearestNodeInfo = AstarPath.active.GetNearest(targetPosition, NNConstraint.Default);
		return nearestNodeInfo.position;
	}
}
