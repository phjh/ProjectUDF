using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area Atttack", menuName = "SO/EnemyMotionState/Attack/Area")]
public class AreaAttackState : EnemyState
{
	[Header("공격 관련 변수")]
	public float ChargeTime = 1f;
	public int RepeatCount = 0;
	public LayerMask WhatIsEnemy;

	private GameObject AttackArea;
	private WaitForSeconds WaitCharge;
	private Coroutine AttackCoroutine;

	public override EnemyState Clone()
	{
		AreaAttackState clone = CloneBase() as AreaAttackState;
		//Setting public values
		clone.AttackArea = enemy.transform.Find("AttackArea").gameObject;
		clone.AttackArea.SetActive(false);
		clone.ChargeTime = ChargeTime;
		clone.RepeatCount = RepeatCount;
		clone.WhatIsEnemy = WhatIsEnemy;

		clone.WaitCharge = new WaitForSeconds(ChargeTime);

		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		Debug.Log("Enter Attack EnemyMotionState");
		enemy.MoveEnemy(Vector2.zero);
		AttackArea.SetActive(false);
		AttackCoroutine = enemy.StartCoroutine(AreaSearch());
	}

	public override void ExitState()
	{
		base.ExitState();
		if (AttackArea)
		{
			AttackArea.SetActive(false);
		}
		if (AttackCoroutine != null)
		{
			enemy.StopCoroutine(AttackCoroutine);
			AttackCoroutine = null;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		if(AttackCoroutine == null)
		{
			enemyStateMachine.ChangeState(enemy.CooldownState);
		}
	}

	private IEnumerator AreaSearch()
	{
		AttackArea.SetActive(false);
		int doCount = 0;
		Collider2D AttackAreaCollider = AttackArea.GetComponent<PolygonCollider2D>();
		Collider2D playerCol;
		do
		{
			AttackArea.SetActive(true);
			Debug.Log($"Attack : {doCount + 1}");
			yield return WaitCharge;
			AttackArea.SetActive(false);
			playerCol = Physics2D.OverlapArea(AttackAreaCollider.bounds.min, AttackAreaCollider.bounds.max, WhatIsEnemy);
			if (playerCol != null)
			{
				Debug.Log("Attack Player");
				/*Player PlayerMain;
				playerCol.gameObject.TryGetComponent(out PlayerMain);
				PlayerMain.GetDamage();*/
			}
			else
			{
				Debug.Log("Fail to Attack Player");
			}
			doCount = doCount + 1;
			yield return null;
		}
		while (RepeatCount > doCount);
		yield return AttackCoroutine = null;
	}
}
