using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	public float DashSpeed;
	public float DashTime;
	public float LockOnTime;
	public LayerMask WhatIsObstacle;

	private Coroutine LockOnCoroutine;
	private Coroutine AttackCoroutine;
	private Vector2 Direction;

	public override EnemyState Clone()
	{
		DashAttackState clone = CloneBase() as DashAttackState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.DashSpeed = DashSpeed;
		clone.DashTime = DashTime;
		clone.LockOnTime = LockOnTime;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.StopAllCoroutines();
		AttackCoroutine = enemy.StartCoroutine(Dash());
		//���� ����
		//AttackCoroutine �۵� / LockOnCoroutine ���� ��� -> LockOnCoroutine �۵� -> ���� ���� ����
		//-> LockOnCoroutine ���� -> ���� �ð� ���� ���� ���� -> �ð� ��� �� �� �̵� �ӵ� 0���� ������ ����
		//AttackCoroutine ���� -> CoolDownState�� ����
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

	private IEnumerator Dash()
	{
		enemy.canAttack = false;
		LockOnCoroutine = enemy.StartCoroutine(LockOnTarget());
		
		yield return LockOnCoroutine;

		float time = 0;
		RaycastHit2D hit;

		while (time <= DashTime)
		{
			hit = Physics2D.Raycast(enemy.transform.position, Direction, 0.8f, WhatIsObstacle);

			// Ray�� � ��ü�� �浹�ϸ鼭 �����ؾ� �մϴ�.
			if (hit.collider != null)
			{
				enemy.MoveEnemy(Vector2.zero);
				break;
			}

			enemy.MoveEnemy(Direction * DashSpeed);
			time += Time.deltaTime;
			yield return null;
		}
		enemy.MoveEnemy(Vector2.zero);
		AttackCoroutine = null;
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		Direction = (enemy.Target.position - enemy.transform.position).normalized;
	}
}
