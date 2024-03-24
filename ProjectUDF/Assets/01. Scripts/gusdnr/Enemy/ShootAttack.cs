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
		//�Ѿ� �߻� �� 4���� * �Ѿ� �ӵ� �� �̵��ϴ� ������� ��ũ��Ʈ �ۼ�
		//���� ���� �ؾߵǳ� �ù�
		//�Ͽ� �ù� �˰� �� ���� �� ����ġ�� �ʹ�
		yield return null;
	}
}
