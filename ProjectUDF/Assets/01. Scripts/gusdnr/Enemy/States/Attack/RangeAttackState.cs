using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/FWayShoot")]
public class RangeAttackState : EnemyState
{
	public int ShootCount;
	public float ShootDelay;
	public BulletMono Bullet;
	
	private Coroutine AttackCoroutine;
	private Vector2 EnemyPos;
	private Vector2 TargetPos;

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
		enemy.MoveEnemy(Vector2.zero);
		TargetPos = enemy.Target.position;
		EnemyPos = enemy.transform.position;
		AttackCoroutine = enemy.StartCoroutine(ShootBullet());
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
		if (AttackCoroutine == null)
		{
			Debug.Log("End Attackcoroutine");
			enemy.StopAllCoroutines();
			enemy.StateMachine.ChangeState(enemy.CooldownState);
		}
	}

	private IEnumerator ShootBullet()
	{
		Vector2 directionToTarget = (TargetPos - EnemyPos).normalized;
		for (int cnt = 0; cnt < ShootCount; cnt++)
		{
			yield return new WaitForSeconds(ShootDelay);
		    BulletMono bullet = PoolManager.Instance.Pop(Bullet.BulletEnum) as BulletMono;
			bullet.gameObject.transform.position = enemy.EnemyRB.position;
			bullet.Shoot(directionToTarget);
		}
		yield return AttackCoroutine = null;
	}
}
