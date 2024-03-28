using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	public float DashSpeed;
	public float DashTime;
	public float LockOnTime;

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
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.StopAllCoroutines();
		AttackCoroutine = enemy.StartCoroutine(DashAttack());
		//���� ����
		//AttackCoroutine �۵� / LockOnCoroutine ���� ��� -> LockOnCoroutine �۵� -> ���� ���� ����
		//-> LockOnCoroutine ���� -> ���� �ð� ���� ���� ���� -> �ð� ��� �� �� �̵� �ӵ� 0���� ������ ����
		//AttackCoroutine ���� -> CoolDownState�� ����
	}

	public override void ExitState()
	{
		enemy.MoveEnemy(Vector2.zero);
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
        if (AttackCoroutine == null)
        {
            enemy.StateMachine.ChangeState(enemy.CooldownState);
        }
    }

	private IEnumerator DashAttack()
	{
		Debug.Log("Start AttackCoroutine");
		enemy.canAttack = false;
		LockOnCoroutine = enemy.StartCoroutine(LockOnTarget());
		
		yield return LockOnCoroutine;

		Debug.Log("Start Dash");
		float time = 0;
		while(time <= DashTime)
		{
			enemy.MoveEnemy(Direction * DashSpeed);
			time += Time.deltaTime;
			yield return null;
		}
		Debug.Log("End Dash");
		enemy.MoveEnemy(Vector2.zero);
	}

	private IEnumerator LockOnTarget()
	{
		Debug.Log("Start LockOn");
		yield return new WaitForSeconds(LockOnTime);
		Direction = (enemy.Target.position - enemy.transform.position).normalized;
		Debug.Log($"End LockOn : [{Direction.x}] [{Direction.y}]");
	}
}
