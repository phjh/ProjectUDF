using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack EnemyMotionState", menuName = "SO/EnemyMotionState/Attack/FWayShoot")]
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
		Vector2 directionToTarget;
		for (int cnt = 0; cnt < ShootCount; cnt++)
		{
			directionToTarget = (TargetPos - EnemyPos).normalized;
			BulletMono bullet = PoolManager.Instance.Pop(Bullet.BulletEnum) as BulletMono;
			bullet.gameObject.transform.position = new Vector3(EnemyPos.x, EnemyPos.y + 0.5f);
			bullet.Shoot(directionToTarget);
			yield return new WaitForSeconds(ShootDelay);
			TargetPos = enemy.Target.position;
		}
		yield return AttackCoroutine = null;
	}
}
