using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive_DropStone", menuName = "SO/Boss/Passive")]
public class PassiveDropStone : BossPassive
{
	[SerializeField] private float AttackRadius = 1.5f;

	private Vector3 TargetPos = Vector3.zero;

	public override void PassiveActive()
	{
		TargetPos = bossMain.TargetTrm.position;
		Debug.Log("���� ���� ���� POP");
		Debug.Log("���� ���� ���� PUSH");
	}
}
