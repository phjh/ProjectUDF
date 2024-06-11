using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase EnemyMotionState", menuName = "SO/EnemyMotionState/Chase/Range")]
public class RangeChaseState : EnemyState
{
	public float movementSpeed;
	private Coroutine WaitCoroutine;
	private Vector2 moveDirection;
	public override EnemyState Clone()
	{
		RangeChaseState clone = (RangeChaseState)CloneBase();
		clone.movementSpeed = movementSpeed;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void ExitState()
	{
		base.ExitState();
		if( WaitCoroutine != null )
		{
			enemy.StopCoroutine( WaitCoroutine );
			WaitCoroutine = null;
		}
	}

	public override void FrameUpdate()
	{
		moveDirection = (enemy.Target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);

		if (enemy.IsWithStrikingDistance)
		{
			if(WaitCoroutine == null) WaitCoroutine = enemy.StartCoroutine(RandomWait());
		}
	}

	private IEnumerator RandomWait()
	{
		enemy.MoveEnemy(moveDirection * (movementSpeed / 2));
		yield return new WaitForSeconds(Random.Range(1, 3));
		enemy.MoveEnemy(Vector2.zero);
		enemy.StateMachine.ChangeState(enemy.AttackState);
	}
}
