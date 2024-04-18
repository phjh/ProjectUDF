using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area Atttack", menuName = "SO/State/Attack/Area")]
public class AreaAttackState : EnemyState
{
	/*public enum AttackType
	{
		None = -1,
		Circle = 0,
		Box = 1,
		Point = 2,
	}

	[Header("공격 방식 설정")]
	public AttackType attackType = AttackType.None;
*/
	[Header("공격 관련 변수")]
	public float Range = 1f;
	public float ChargeTime = 1f;
	public int RepeatCount = 0;
	public LayerMask WhatIsEnemy;

	private WaitForSeconds WaitCharge;

	public override EnemyState Clone()
	{
		AreaAttackState clone = CloneBase() as AreaAttackState;
		//Setting public values
		//clone.attackType = attackType;
		clone.Range = Range;
		clone.ChargeTime = ChargeTime;
		clone.RepeatCount = RepeatCount;
		clone.WhatIsEnemy = WhatIsEnemy;

		clone.WaitCharge = new WaitForSeconds(ChargeTime);

		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.MoveEnemy(Vector2.zero);
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();


	}

	private IEnumerator AreaSearch()
	{
		int doCount = 0;
		do
		{
			yield return WaitCharge;
			Collider2D PlayerCollider = Physics2D.OverlapCircle(enemy.EnemyRB.position, Range, WhatIsEnemy);

			if (PlayerCollider != null)
			{
				Player PlayerMain;
				PlayerCollider.gameObject.TryGetComponent(out PlayerMain);
				PlayerMain.GetDamage();
			}
			else
			{
				Debug.Log("Fail to Attack Player");
			}
			doCount = doCount + 1;
		}
		while (RepeatCount <= doCount);
		yield return null;
	}

}
