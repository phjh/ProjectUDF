using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour, IAttack
{
	[SerializeField] private float dashSpeed = 8f; // 돌진 속도
	[SerializeField] private float dashDuration = 1.5f;

	public void DoingAttack(EnemyBase eb)
	{
		if (eb != null && eb.Target != null)
		{
			StartCoroutine(Dash(eb));
		}
	}

	IEnumerator Dash(EnemyBase eb)
	{
		Vector2 direction = (eb.Target.position - eb.transform.position).normalized;
		// Rigidbody2D.velocity를 이용해 이동
		eb.EnemyRB.velocity = direction * dashSpeed;
		// 대쉬 지속 시간 동안 대기
		yield return new WaitForSeconds(dashDuration);
		// 대쉬 종료 후 속도 초기화
		eb.EnemyRB.velocity = Vector2.zero;

		// 여기에서 다양한 로직 추가 가능
	}
}
