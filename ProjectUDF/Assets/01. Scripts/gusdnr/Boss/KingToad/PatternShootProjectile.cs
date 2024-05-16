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
			//Ǯ���� ������Ʈ ���� �۵� �κ�
		}
	}

	public override void EnterPattern()
	{
		TargetPosition = bossMain.TargetTrm.position;
		IsActive = true;
		//������Ʈ Ǯ���� �̿��� �߻��� ������Ʈ �̸� ����
	}

	public override void ExitPattern()
	{
		IsActive = false;
	}
}
