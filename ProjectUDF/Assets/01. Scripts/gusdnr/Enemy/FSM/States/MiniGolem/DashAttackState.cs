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
		// 추가적인 초기화가 필요한 경우 여기서 설정
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
		//공격 순서
		//AttackCoroutine 작동 / LockOnCoroutine 종료 대기 -> LockOnCoroutine 작동 -> 돌진 방향 지정
		//-> LockOnCoroutine 종료 -> 일정 시간 동안 돌진 실행 -> 시간 경과 후 적 이동 속도 0으로 변경해 정지
		//AttackCoroutine 종료 -> CoolDownState로 변경
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
