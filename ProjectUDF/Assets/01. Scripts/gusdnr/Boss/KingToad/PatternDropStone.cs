using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_DropStone", menuName = "SO/Boss/KingToad/Pattern_DropStone")]
public class PatternDropStone : BossPattern
{
	private Vector3 TargetPos = Vector3.zero;

	private Coroutine DoingPattern;

	public override void EnterPattern()
	{
		bossMain.IsAttack = true;
		TargetPos = bossMain.TargetTrm.position;
	}

	public override void ActivePattern()
	{
		TargetPos = bossMain.TargetTrm.position;

		Debug.Log("���� ���� ���� POP");
		Debug.Log($"Tarrget : {TargetPos}");
		Debug.Log("���� ���� ���� PUSH");
	}

	public override void ExitPattern()
	{
		bossMain.IsAttack = false;
	}
}
