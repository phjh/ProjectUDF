using System.Collections;
using UnityEngine;
using DG.Tweening;

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
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.DashSpeed = DashSpeed;
		clone.DashTime = DashTime;
		clone.LockOnTime = LockOnTime;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		DOTween.Init();
		base.EnterState();
		enemy.StopAllCoroutines();
		AttackCoroutine = enemy.StartCoroutine(Dash());
		//공격 순서
		//AttackCoroutine 작동 / LockOnCoroutine 종료 대기 -> LockOnCoroutine 작동 -> 돌진 방향 지정
		//-> LockOnCoroutine 종료 -> 일정 시간 동안 돌진 실행 -> 시간 경과 후 적 이동 속도 0으로 변경해 정지
		//AttackCoroutine 종료 -> CoolDownState로 변경
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

		RaycastHit2D CalculateObstacle;
		CalculateObstacle = Physics2D.Raycast(enemy.transform.position, Direction, 1f, WhatIsObstacle);
		Vector2 endPoint = CalculateObstacle.point;
		
		var dashSeq = DOTween.Sequence();
		
		dashSeq.Append(enemy.transform.DOMove(endPoint, DashTime).SetEase(Ease.OutCubic));

		dashSeq.Play().OnComplete(() =>
		{
			enemy.MoveEnemy(Vector2.zero);
			AttackCoroutine = null;
		});
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		Direction = (enemy.Target.position - enemy.transform.position).normalized;
	}
}
