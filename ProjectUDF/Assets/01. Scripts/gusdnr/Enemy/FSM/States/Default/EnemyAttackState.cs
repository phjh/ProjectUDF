using System.Collections;
using UnityEngine;

public abstract class EnemyAttackState : EnemyState
{
	private Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴

	public override void EnterState()
	{
		base.EnterState();
		attackCoroutine = enemy.StartCoroutine(AttackRoutine());
	}

	public override void ExitState()
	{
		base.ExitState();

		// 공격 코루틴이 실행 중이라면 중지
		if (attackCoroutine != null)
		{
			enemy.StopCoroutine(attackCoroutine);
			attackCoroutine = null;
		}
	}

	public virtual void ExecuteAttack() { }
	public virtual bool IsAttackFinished() { return true; }

	private IEnumerator AttackRoutine()
	{
		enemy.canAttack = false;
		ExecuteAttack(); // 공격 패턴 실행

		// 여기서 공격이 끝날 때까지 대기
		while (!IsAttackFinished())
		{
			yield return null;
		}

		// 공격이 끝나면 다른 상태로 전환
		if (!enemy.isDead) // 적이 살아 있는지 확인
		{
			enemyStateMachine.ChangeState(enemy.CooldownState);
		}
	}
}
