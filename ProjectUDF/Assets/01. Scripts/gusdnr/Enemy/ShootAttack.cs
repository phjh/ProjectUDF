using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : AtkPatternMono
{
    [SerializeField] private GameObject Bullet;
	[SerializeField] private float bulletSpeed;
	public float AttackTime;

	public override void DoingAttack(EnemyBase eb)
	{
		if (eb != null && eb.target != null)
		{
			eb.aiPath.destination = eb.EnemyPos;
			StartCoroutine(ShootStone(eb));
		}
	}

	private IEnumerator ShootStone(EnemyBase eb)
	{
		eb.aiPath.isStopped = true;
		eb.isAttacking = true;
		eb.aiPath.destination = eb.EnemyPos;
		for (int i = 0; i < 3; i++)
		{

			//�Ѿ� �߻� �� 4���� * �Ѿ� �ӵ� �� �̵��ϴ� ������� ��ũ��Ʈ �ۼ�
			yield return new WaitForSeconds(1.5f);
		}
		yield return new WaitForSeconds(AttackTime);
		StartCoroutine(eb.CooldownAttack());
	}
}
