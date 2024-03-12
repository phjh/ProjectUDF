using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour, IAttack
{
	[SerializeField] private float dashSpeed = 8f; // ���� �ӵ�
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
		// Rigidbody2D.velocity�� �̿��� �̵�
		eb.EnemyRB.velocity = direction * dashSpeed;
		// �뽬 ���� �ð� ���� ���
		yield return new WaitForSeconds(dashDuration);
		// �뽬 ���� �� �ӵ� �ʱ�ȭ
		eb.EnemyRB.velocity = Vector2.zero;

		// ���⿡�� �پ��� ���� �߰� ����
	}
}
