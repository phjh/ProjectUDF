using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/FWayShoot")]
public class FWayShootAttackState : EnemyState
{
	public int ShootCount;
	public float ShootDelay;
	public GameObject Stone;
	
	private Coroutine AttackCoroutine;
	private Vector2 Direction;

	public override EnemyState Clone()
	{
		FWayShootAttackState clone = (FWayShootAttackState)CloneBase();
		clone.ShootCount = ShootCount;
		clone.ShootDelay = ShootDelay;
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
		for (int cnt = 0; cnt < ShootCount; cnt++)
		{
			//총알 발사하는 부분 작성
			enemy.EnemyRB.AddForce(Direction * 2);
			yield return new WaitForSeconds(ShootDelay);
		}
		AttackCoroutine = null;
	}
}
