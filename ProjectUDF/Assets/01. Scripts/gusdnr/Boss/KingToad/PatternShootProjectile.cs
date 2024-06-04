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

	public override void EnterPattern()
	{
		Target = bossMain.TargetTrm;
		//������Ʈ Ǯ���� �̿��� �߻��� ������Ʈ �̸� ����
		bossMain.StartCoroutine(Shooting());
	}

	public override void ExitPattern()
	{
		IsActive = false;
		bossMain.SetState(NextState[0]);
	}

	private IEnumerator Shooting()
	{
		for (int i = 0; i < ShootCount; i++)
		{
			ProjectileObjet.CustomInstantiate(bossMain.transform.position, ProjectileObjet.BulletEnum);
			ProjectileObjet.Shoot(Target);
			yield return new WaitForSeconds(ShootTerm);
		}
		IsActive = false;
	}
}
