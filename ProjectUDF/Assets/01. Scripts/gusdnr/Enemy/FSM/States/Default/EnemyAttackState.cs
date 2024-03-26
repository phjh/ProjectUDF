using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
	private List<EnemyAttackPatternBase> attackPatterns = new List<EnemyAttackPatternBase>(); // 다양한 공격 패턴을 담을 리스트
	private Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴

	public EnemyAttackState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
		// 개체 내부에서 EnemyAttackPatternBase를 상속받은 모든 스크립트를 찾아 리스트에 추가
		EnemyAttackPatternBase[] foundPatterns = enemy.GetComponents<EnemyAttackPatternBase>();
		foreach (EnemyAttackPatternBase pattern in foundPatterns)
		{
			attackPatterns.Add(pattern);
		}
	}

	public override void EnterState()
	{
		base.EnterState();

		// 원하는 공격 패턴 선택 및 실행
		if (attackPatterns.Count > 0)
		{
			int randomIndex = Random.Range(0, attackPatterns.Count);
			attackCoroutine = enemy.StartCoroutine(AttackRoutine(attackPatterns[randomIndex]));
		}
		else
		{
			Debug.LogWarning("No attack patterns found in the enemy object.");
			enemyStateMachine.ChangeState(new EnemyIdleState(enemy, enemyStateMachine));
		}
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
		attackPattern.ExecuteAttack(); // 공격 패턴 실행

		// 여기서 공격이 끝날 때까지 대기
		while (!attackPattern.IsAttackFinished())
		{
			yield return null;
		}

		// 공격이 끝나면 다른 상태로 전환
		enemyStateMachine.ChangeState(new EnemyIdleState(enemy, enemyStateMachine));
	}
}