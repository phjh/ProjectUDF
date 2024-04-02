using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/FWayShoot")]
public class FWayShootAttackState : EnemyState
{
	public int ShootCount;
	public float AttackDelay;
	public GameObject Stone;
	
	private Coroutine AttackCoroutine;
	private Vector2 Direction;

	public override EnemyState Clone()
	{
		FWayShootAttackState clone = (FWayShootAttackState)CloneBase();
		clone.ShootCount = ShootCount;
		clone.AttackDelay = AttackDelay;
		clone.Stone = Stone;
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
			ToadRock Rock = PoolManager.Instance.Pop(PoolingType.ToadBullet).GetComponent<ToadRock>();
			Rock.transform.position = enemy.transform.position;
			switch (cnt)
			{
				case 0: dir = Vector2.up; break;
				case 1: dir = Vector2.left; break;
				case 2: dir = Vector2.down; break;
				case 3: dir = Vector2.right; break;
			}
			Rock.ShootingRock(dir);
		}
		yield return new WaitForSeconds(AttackDelay);
		AttackCoroutine = null;
	}
}
