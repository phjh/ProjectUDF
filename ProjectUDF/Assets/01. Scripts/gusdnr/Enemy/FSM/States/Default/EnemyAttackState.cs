using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
	private EnemyAttackPatternBase attackPattern;// 공격 패턴
	private Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴

	public EnemyAttackState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
		attackPattern = enemy.GetComponent<EnemyAttackPatternBase>();
	}

	public override void EnterState()
	{
		base.EnterState();
		// 원하는 공격 패턴 선택 및 실행
		attackCoroutine = enemy.StartCoroutine(AttackRoutine(attackPattern));
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

	private IEnumerator AttackRoutine(EnemyAttackPatternBase attackPattern)
	{
		enemy.canAttack = false;
		attackPattern.ExecuteAttack(); // 공격 패턴 실행

		// 여기서 공격이 끝날 때까지 대기
		while (!attackPattern.IsAttackFinished())
		{
			yield return null;
		}

		// 공격이 끝나면 다른 상태로 전환
		if (!enemy.isDead) // 적이 살아 있는지 확인
		{
			enemyStateMachine.ChangeState(enemy.IdleState);
			enemy.StartCoroutine(enemy.StartAttackCooldown());
		}
	}
}