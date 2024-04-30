using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/FWayShoot")]
public class RangeAttackState : EnemyState
{
	public int ShootCount;
	public float ShootDelay;
	public GameObject Bullet;
	
	private Coroutine AttackCoroutine;
	private Vector2 Direction;

	public override EnemyState Clone()
	{
		RangeAttackState clone = (RangeAttackState)CloneBase();
		clone.ShootCount = ShootCount;
		clone.ShootDelay = ShootDelay;
		clone.Bullet = Bullet;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.StopAllCoroutines();
		AttackCoroutine = enemy.StartCoroutine(ShootFWay());
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.MoveEnemy(Vector2.zero);
		if (AttackCoroutine != null)
		{
			enemy.StopCoroutine(AttackCoroutine);
			AttackCoroutine = null;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		Direction = (enemy.Target.position - enemy.transform.position).normalized;
		if (AttackCoroutine == null)
		{
			Debug.Log("End Attackcoroutine");
			enemy.StopAllCoroutines();
			enemy.StateMachine.ChangeState(enemy.CooldownState);
		}
	}

	private IEnumerator ShootFWay()
	{
		Vector2 dir = Vector2.zero;
		for (int cnt = 0; cnt < ShootCount; cnt++)
		{
			switch (cnt)
			{
				case 0: dir = Vector2.up; break;
				case 1: dir = Vector2.left; break;
				case 2: dir = Vector2.down; break;
				case 3: dir = Vector2.right; break;
			}
			yield return new WaitForSeconds(ShootDelay);
		}
		AttackCoroutine = null;
	}
}
