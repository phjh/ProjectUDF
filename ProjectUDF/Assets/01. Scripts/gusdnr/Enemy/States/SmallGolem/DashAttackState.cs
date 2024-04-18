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
	private Vector2 EnemyPos;
	private Vector2 EndPoint;

	public override EnemyState Clone()
	{
		DashAttackState clone = CloneBase() as DashAttackState;
		// ì¶”ê??ì¸ ì´ˆê¸°?”ê? ?„ìš”??ê²½ìš° ?¬ê¸°???¤ì •
		clone.LockOnTime = LockOnTime;
		clone.DashTime = DashTime;
		clone.DashDistance = DashDistance;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		DOTween.Init();
		TargetPos = Vector2.zero;
		Debug.Log($"Enter Target Pos : [X: {TargetPos.x}] [Y: {TargetPos.y}]");
		EndPoint = Vector2.zero;
		EnemyPos = enemy.transform.position;
		Debug.Log($"Enter End Pos : [X: {EndPoint.x}] [Y: {EndPoint.y}]");
		enemy.StopAllCoroutines();
		AttackCoroutine = enemy.StartCoroutine(Dash());

		//ê³µê²© ?œì„œ
		//AttackCoroutine ?‘ë™ / LockOnCoroutine ì¢…ë£Œ ?€ê¸?-> LockOnCoroutine ?‘ë™ -> ?Œì§„ ë°©í–¥ ì§€??
		//-> LockOnCoroutine ì¢…ë£Œ -> ?¼ì • ?œê°„ ?™ì•ˆ ?Œì§„ ?¤í–‰ -> ?œê°„ ê²½ê³¼ ?????´ë™ ?ë„ 0?¼ë¡œ ë³€ê²½í•´ ?•ì?
		//AttackCoroutine ì¢…ë£Œ -> CoolDownStateë¡?ë³€ê²?
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

		RaycastHit2D HitObstacle = Physics2D.Raycast(EnemyPos, TargetPos, DashDistance, WhatIsObstacle);
		Debug.Log($"Is Checking Obstacle : {(bool)HitObstacle}");
		if (HitObstacle)
		{
			EndPoint = HitObstacle.point;
			EndPoint.x = (EndPoint.x > EnemyPos.x) ? EndPoint.x - 1.1f : EndPoint.x + 1.1f;
			EndPoint.y = (EndPoint.y > EnemyPos.y) ? EndPoint.y - 0.3f : EndPoint.y + 0.3f;
		}
		else if (!HitObstacle)
		{
			Vector2 directionToTarget = (TargetPos - EnemyPos).normalized;
			EndPoint = EnemyPos + directionToTarget * DashDistance * (directionToTarget.x >= 0 ? 1 : -1);
			Debug.Log($"End Pos.normal : [{(EnemyPos - TargetPos).normalized.x}] [{(EnemyPos - TargetPos).normalized.normalized.y}]");
		}
		Debug.DrawRay(EnemyPos, TargetPos, Color.cyan);
		Debug.DrawRay(EnemyPos, EndPoint, Color.magenta, DashDistance);
		Debug.Log($"End Point : [X: {EndPoint.x}] [Y: {EndPoint.y}]");
		enemy.CheckForFacing(EndPoint.normalized);
		yield return new WaitForSeconds(1);
		var dashSeq = DOTween.Sequence();

		dashSeq.Append(enemy.transform.DOMove(EndPoint, DashTime).SetEase(Ease.OutCirc));

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

		Debug.DrawRay(EnemyPos, TargetPos, Color.green, DashDistance);
	}
}
