using System.Collections;
using UnityEngine;

public abstract class EnemyAttackState : EnemyState
{
	private Coroutine attackCoroutine; // ���� ���� ���� ���� �ڷ�ƾ

	public override void EnterState()
	{
		base.EnterState();
		attackCoroutine = enemy.StartCoroutine(AttackRoutine());
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

	public virtual void ExecuteAttack() { }
	public virtual bool IsAttackFinished() { return true; }

	private IEnumerator AttackRoutine()
	{
		enemy.canAttack = false;
		ExecuteAttack(); // ���� ���� ����

		// ���⼭ ������ ���� ������ ���
		while (!IsAttackFinished())
		{
			yield return null;
		}

		// ������ ������ �ٸ� ���·� ��ȯ
		if (!enemy.isDead) // ���� ��� �ִ��� Ȯ��
		{
			enemyStateMachine.ChangeState(enemy.CooldownState);
		}
	}
}
