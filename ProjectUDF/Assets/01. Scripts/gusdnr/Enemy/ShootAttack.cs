using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : AtkPatternMono
{
    [SerializeField] private GameObject Bullet;
	[SerializeField] private float bulletSpeed;

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
		eb.aiPath.isStopped = false;
		for(int i = 0; i < 0 ; i++)
		{
			
		}
		//총알 발사 시 4방향 * 총알 속도 로 이동하는 방식으로 스크립트 작성
		//내가 굳이 해야되냐 시발
		//하오 시발 알거 개 많네 걍 때려치고 싶다
		yield return null;
	}
}
