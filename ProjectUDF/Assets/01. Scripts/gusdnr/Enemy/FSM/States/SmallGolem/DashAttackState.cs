using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	[Header("Ready Values")]
	public float LockOnTime;
	[Header("Dash Values")]
	public float DashTime;
	public float DashDistance;
	public LayerMask WhatIsObstacle;

	private Coroutine LockOnCoroutine;
	private Coroutine AttackCoroutine;
	private Vector2 TargetPos;
	private Vector2 EndPoint;

	public override EnemyState Clone()
	{
		DashAttackState clone = CloneBase() as DashAttackState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.LockOnTime = LockOnTime;
		clone.DashTime = DashTime;
		clone.DashDistance = DashDistance;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		DOTween.Init();
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

		RaycastHit2D HitObstacle = Physics2D.Raycast(enemy.transform.position, TargetPos, DashDistance, WhatIsObstacle);
		Debug.Log($"Is Checking Obstacle : {HitObstacle}");
		if (HitObstacle)
		{
			EndPoint = HitObstacle.point;
			EndPoint.x = (EndPoint.x > enemy.transform.position.x) ? EndPoint.x - 1f : EndPoint.x + 1f;	
			EndPoint.y = (EndPoint.y > enemy.transform.position.y) ? EndPoint.y - 0.5f : EndPoint.y + 0.5f;
		}
		else if (!HitObstacle)
		{
			EndPoint = new Vector2(enemy.transform.position.x + TargetPos.x, enemy.transform.position.y + TargetPos.y) * DashDistance;
		}
		Debug.Log($"End Point : [X: {EndPoint.x}] [Y: {EndPoint.y}]");
		yield return new WaitForSeconds(1);
		enemy.CheckForFacing(EndPoint.normalized);
		var dashSeq = DOTween.Sequence();
		
		dashSeq.Append(enemy.transform.DOMove(EndPoint, DashTime).SetEase(Ease.OutCubic));

		dashSeq.Play().OnComplete(() =>
		{
			AttackCoroutine = null;
		});
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		TargetPos = enemy.Target.position;
		Debug.Log($"Target Pos : [X: {TargetPos.x}] [Y: {TargetPos.y}]");
	}
}
