using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_ShootProjectile", menuName = "SO/Boss/KingToad/Pattern_ShootProjectile")]
public class PatternShootProjectile : BossPattern
{
    [SerializeField] private BulletMono ProjectileObjet;
    [SerializeField] private float ShootTerm = 0.5f;

	[SerializeField] private Transform[] ShootPositions;
	
	private Transform Target;
	private Coroutine AttackCoroutine;

	public override void EnterPattern()
	{
		Target = bossMain.TargetTrm;
		IsActive = true;
		//������Ʈ Ǯ���� �̿��� �߻��� ������Ʈ �̸� ����
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
		bossMain.SetState(NextState[0]);
	}

	private IEnumerator Shooting()
	{
		for (int i = 0; i < ShootPositions.Length; i++)
		{
			ProjectileObjet.CustomInstantiate(ShootPositions[i].position, ProjectileObjet.BulletEnum);
			ProjectileObjet.Shoot(Target);
			yield return new WaitForSeconds(ShootTerm);
		}
	}
}
