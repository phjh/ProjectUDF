using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
	private Vector3 _targetPos;
	private Vector3 _direction;

	public float MaxMoveTime;

	private bool endTimer = false;
	private Coroutine timerCoroutine;

	public EnemyIdleState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
	}

	public override void AnimationTriggerEvent(EnemyMain.AnimationTriggerType triggerType)
	{
		base.AnimationTriggerEvent(triggerType);
	}

	public override void EnterState()
	{
		base.EnterState();

		_targetPos = GetRandomPointInCircle();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		_direction = (_targetPos - enemy.transform.position).normalized;
		enemy.MoveEnemy(_direction * enemy.RandomMovementSpeed);

		if(((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f) || endTimer)
		{
			_targetPos = GetRandomPointInCircle();
		}
	}

	public override void PhtsicsUpdate()
	{
		base.PhtsicsUpdate();
	}

	private Vector3 GetRandomPointInCircle()
	{
		if(timerCoroutine != null) enemy.StopCoroutine(timerCoroutine);
		timerCoroutine = enemy.StartCoroutine(MoveTimer());
		return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * enemy.RandomMovementRange;
	}

	private IEnumerator MoveTimer()
	{
		endTimer = false;
		float curTime = 0;
		while (MaxMoveTime > curTime)
		{
			curTime += Time.deltaTime;
			yield return null;
		}
		endTimer = true;
	}
}
