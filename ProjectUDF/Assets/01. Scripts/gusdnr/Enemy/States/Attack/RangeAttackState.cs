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
	private Vector3 EnemyPos;
	private Vector3 TargetPos;

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
			// 타겟 위치 계산
			Vector3 playerPos = enemy.Target.position;
			Vector3 directionToPlayer = (playerPos - EnemyPos).normalized;
			Vector3 perpendicularVector = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0);

			Vector3 leftTarget = playerPos + perpendicularVector;
			Vector3 rightTarget = playerPos - perpendicularVector;

			float distanceToLeft = Vector3.Distance(leftTarget, EnemyPos);
			float distanceToRight = Vector3.Distance(rightTarget, EnemyPos);

			if (distanceToLeft < distanceToRight)
			{
				TargetPos = leftTarget;
			}
			else
			{
				TargetPos = rightTarget;
			}
			enemy.CheckForFacing(TargetPos);

			directionToTarget = (TargetPos - EnemyPos).normalized;
			BulletMono bullet = PoolManager.Instance.Pop(Bullet.BulletEnum) as BulletMono;
			bullet.gameObject.transform.position = new Vector3(EnemyPos.x, EnemyPos.y + 0.5f);
			bullet.Shoot(directionToTarget);
			yield return new WaitForSeconds(ShootDelay);
		}
		yield return AttackCoroutine = null;
	}
}
