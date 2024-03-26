using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : EnemyAttackPatternBase
{
	private Coroutine attackCoroutine;

	private Transform target;
	private bool lockOn = false;
	public float dashSpeed = 10f;
	public float dashTime = 3f;

	public float lockOnTime = 2f;

	public DashAttack(EnemyMain enemy) : base(enemy)
	{
	}

	public override void ExecuteAttack()
	{
		attackCoroutine = enemy.StartCoroutine(OnDash());
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
		Vector2 direction = (target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(direction * dashSpeed);
		yield return new WaitForSeconds(dashTime);
		enemy.MoveEnemy(Vector2.zero);
	}

	private IEnumerator LockOn()
	{
		yield return new WaitForSeconds(lockOnTime);
		target = GameManager.Instance.player.transform;
		lockOn = true;
	}
}
