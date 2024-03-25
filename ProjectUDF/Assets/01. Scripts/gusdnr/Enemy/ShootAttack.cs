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

			//총알 발사 시 4방향 * 총알 속도 로 이동하는 방식으로 스크립트 작성
			yield return new WaitForSeconds(1.5f);
		}
		yield return new WaitForSeconds(AttackTime);
		StartCoroutine(eb.CooldownAttack());
	}
}
