using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShootProjectile", menuName = "SO/Boss/KingToad/Pattern_ShootProjectile")]
public class PatternShootProjectile : BossPattern
{
    [SerializeField] private BulletMono ProjectileObjet;
    [SerializeField] private float ShootTerm = 0.5f;

	[SerializeField] private int ShootCount = 5;
	
	private Transform Target;
	private Coroutine AttackCoroutine = null;

	public override void EnterPattern()
	{
		Target = bossMain.TargetTrm;
		//오브젝트 풀링을 이용해 발사할 오브젝트 미리 생성
		AttackCoroutine = bossMain.StartCoroutine(Shooting());
	}

	public override void ActivePattern()
	{
		if (AttackCoroutine == null)
		{
			bossMain.SetState(NextState[0]);
		}
	}

	public override void ExitPattern()
	{
	}

	private IEnumerator Shooting()
	{
		for (int i = 0; i < ShootCount; i++)
		{
			BulletMono follow = ProjectileObjet.InstantiateReturnObject(bossMain.transform.position, ProjectileObjet.BulletEnum).GetComponent<BulletMono>();
			follow.Shoot(Target);
			Debug.Log($"[{ShootCount}]");
			yield return new WaitForSeconds(ShootTerm);
		}
		IsActive = false;

		AttackCoroutine = null;
	}
}
