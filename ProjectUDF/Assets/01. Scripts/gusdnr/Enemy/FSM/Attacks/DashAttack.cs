/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : AttackPatternBase
{
	private Coroutine attackCoroutine;

	private Transform target;
	private Vector2 direction;
	
	private bool lockOn = false;
	public float dashSpeed = 10f;
	public float dashTime = 3f;


	public float lockOnTime = 2f;

	public DashAttack(EnemyMain enemy) : base(enemy)
	{
	}

	public override void ExecuteAttack()
	{
		target = GameManager.Instance.player.transform;
		direction = (target.position - enemy.transform.position).normalized;
		enemy.EnemyRB.AddForce(direction * dashSpeed, ForceMode2D.Force);
	}

	public override bool IsAttackFinished()
	{
		return attackCoroutine == null;
	}

	private IEnumerator OnDash()
	{
		enemy.StartCoroutine(LockOn());
		while(!lockOn)
		{
			yield return null;
		}
		Debug.Log("Dash Start");
		enemy.MoveEnemy(direction * dashSpeed);
		yield return new WaitForSeconds(dashTime);
		Debug.Log("Dash End");
		enemy.MoveEnemy(Vector2.zero);
	}

	private IEnumerator LockOn()
	{
		Debug.Log("Lock On Start");
		yield return new WaitForSeconds(lockOnTime);
		if (GameManager.Instance.player != null)
		{
			target = GameManager.Instance.player.transform;
			direction = (target.position - enemy.transform.position).normalized;
			lockOn = true;
		}
		else
		{
			Debug.LogError("Player object not found in GameManager.");
			if (attackCoroutine != null) // null 검사 추가
				enemy.StopCoroutine(OnDash());
		}
	}
}
*/