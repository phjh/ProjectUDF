using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShootProjectile", menuName = "SO/Boss/KingToad/Pattern_ShootProjectile")]
public class PatternShootProjectile : BossPattern
{
    //[SerializeField] private GameObject ProjectileObjet;
    [SerializeField] private int ShootCount = 3;

	private Vector3 TargetPosition;
	private Coroutine AttackCoroutine;

	public override void EnterPattern()
	{
		TargetPosition = bossMain.TargetTrm.position;
		IsActive = true;
		//오브젝트 풀링을 이용해 발사할 오브젝트 미리 생성
		AttackCoroutine = bossMain.StartCoroutine(Shooting());
	}

	public override void ActivePattern()
	{
		if(AttackCoroutine == null)
		{
			ExitPattern();
		}
	}

	public override void ExitPattern()
	{
		IsActive = false;
		bossMain.SetState(BossMain.BossState.Cooldown);
	}

	private IEnumerator Shooting()
	{
		for (int i = 0; i < ShootCount; i++)
		{
			Debug.Log("Shoot Projectile");
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}
}
