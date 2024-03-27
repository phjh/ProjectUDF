using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
	private EnemyAttackPatternBase attackPattern;// ���� ����
	private Coroutine attackCoroutine; // ���� ���� ���� ���� �ڷ�ƾ

	public EnemyAttackState(EnemyMain enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
	{
		attackPattern = enemy.GetComponent<EnemyAttackPatternBase>();
	}

	public override void EnterState()
	{
		base.EnterState();
		// ���ϴ� ���� ���� ���� �� ����
		attackCoroutine = enemy.StartCoroutine(AttackRoutine(attackPattern));
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
		enemy.canAttack = false;
		attackPattern.ExecuteAttack(); // ���� ���� ����

		// ���⼭ ������ ���� ������ ���
		while (!attackPattern.IsAttackFinished())
		{
			yield return null;
		}

		// ������ ������ �ٸ� ���·� ��ȯ
		if (!enemy.isDead) // ���� ��� �ִ��� Ȯ��
		{
			enemyStateMachine.ChangeState(enemy.IdleState);
			enemy.StartCoroutine(enemy.StartAttackCooldown());
		}
	}
}