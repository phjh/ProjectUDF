using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShootProjectile", menuName = "SO/Boss/Pattern")]
public class PatternShootProjectile : BossPattern
{
    [SerializeField] private GameObject ProjectileObjet;
    [SerializeField] private int ShootCount = 3;

	private Vector3 TargetPosition;

	public override void ActivePattern()
	{
		for(int c = 0; c < ShootCount; c++)
		{
			//풀링한 오브젝트 실제 작동 부분
		}
	}

	public override void EnterPattern()
	{
		TargetPosition = bossMain.TargetTrm.position;
		IsActive = true;
		//오브젝트 풀링을 이용해 발사할 오브젝트 미리 생성
	}

	public override void ExitPattern()
	{
		IsActive = false;
	}
}
