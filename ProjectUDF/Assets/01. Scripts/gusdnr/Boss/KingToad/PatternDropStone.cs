using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_DropStone", menuName = "SO/Boss/KingToad/Pattern_DropStone")]
public class PatternDropStone : BossPattern
{
	[SerializeField] private float AttackRadius = 1.5f;

	private Vector3 TargetPos = Vector3.zero;

	public override void Initialize(BossMain bossMain)
	{
	}

	public override void EnterPattern()
	{
		bossMain.IsAttack = true;
	}

	public override void ActivePattern()
	{
		TargetPos = bossMain.TargetTrm.position;
		CircleCollider2D AttackRange = new CircleCollider2D();
		AttackRange.radius = AttackRadius;
		Debug.Log("���� ���� ���� POP");
		Debug.Log("���� ���� ���� PUSH");
	}

	public override void ExitPattern()
	{
		bossMain.IsAttack = false;
		bossMain.SetState(BossMain.BossState.Cooldown);
	}
}
