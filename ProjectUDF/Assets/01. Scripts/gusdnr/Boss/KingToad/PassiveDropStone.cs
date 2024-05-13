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
		Debug.Log("보스 낙석 장판 POP");
		Debug.Log("보스 낙석 장판 PUSH");
	}
}
