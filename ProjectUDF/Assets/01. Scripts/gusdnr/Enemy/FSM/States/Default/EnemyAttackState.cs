using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
	private List<EnemyAttackPatternBase> attackPatterns = new List<EnemyAttackPatternBase>(); // �پ��� ���� ������ ���� ����Ʈ
	private Coroutine attackCoroutine; // ���� ���� ���� ���� �ڷ�ƾ

	public EnemyAttackState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
		// ��ü ���ο��� EnemyAttackPatternBase�� ��ӹ��� ��� ��ũ��Ʈ�� ã�� ����Ʈ�� �߰�
		EnemyAttackPatternBase[] foundPatterns = enemy.GetComponents<EnemyAttackPatternBase>();
		foreach (EnemyAttackPatternBase pattern in foundPatterns)
		{
			attackPatterns.Add(pattern);
		}
	}

	public override void EnterState()
	{
		base.EnterState();

		// ���ϴ� ���� ���� ���� �� ����
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

		// ���� �ڷ�ƾ�� ���� ���̶�� ����
		if (attackCoroutine != null)
		{
			enemy.StopCoroutine(attackCoroutine);
			attackCoroutine = null;
		}
	}

	private IEnumerator AttackRoutine(EnemyAttackPatternBase attackPattern)
	{
		attackPattern.ExecuteAttack(); // ���� ���� ����

		// ���⼭ ������ ���� ������ ���
		while (!attackPattern.IsAttackFinished())
		{
			yield return null;
		}

		// ������ ������ �ٸ� ���·� ��ȯ
		enemyStateMachine.ChangeState(new EnemyIdleState(enemy, enemyStateMachine));
	}
}