using System.Collections;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Attack State", menuName = "SO/State/Attack/Dash")]
public class DashAttackState : EnemyState
{
	[Header("Ready Values")]
	public float LockOnTime;
	[Header("Dash Values")]
	public float DashTime;
	public float DashDistance;
	public LayerMask WhatIsEnemy;
	public LayerMask WhatIsObstacle;

	private GridGraph gridGraph;

	private Coroutine LockOnCoroutine;
	private Coroutine AttackCoroutine;
	private Vector2 TargetPos;
	private Vector2 EnemyPos;
	private Vector2 EndPoint;

	public override EnemyState Clone()
	{
		DashAttackState clone = CloneBase() as DashAttackState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.LockOnTime = LockOnTime;
		clone.DashTime = DashTime;
		clone.DashDistance = DashDistance;
		clone.WhatIsEnemy = WhatIsEnemy;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		DOTween.Init();
		TargetPos = Vector2.zero;
		EndPoint = Vector2.zero;
		//EnemyPos = enemy.EnemyRB.position;
		enemy.StopAllCoroutines();
		EnemyPos = enemy.EnemyRB.position;

		gridGraph = AstarPath.active.data.gridGraph;

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
			enemyStateMachine.ChangeState(enemy.CooldownState);
        }
    }

	private IEnumerator Dash()
	{
		enemy.canAttack = false;
		LockOnCoroutine = enemy.StartCoroutine(LockOnTarget());
		
		yield return LockOnCoroutine;

		//�� -> ��ǥ ������ ���� ��Ȯ�� ���� ���� ����
		Vector2 directionToTarget = (TargetPos - EnemyPos).normalized;
		//�� -> ��ǥ ������ ���� Ray�� ��� �߰��� ��ֹ��� �ִ��� �˻�
		RaycastHit2D HitObstacle = Physics2D.Raycast(EnemyPos, directionToTarget, DashDistance, WhatIsObstacle);
		if (HitObstacle)
		{
			//��ֹ� ��ġ�� ����
			Vector2 HitPos = HitObstacle.point;
			//�ݶ��̴� ũ�⸸ŭ ���־� A* Graph ��Ż ����
			HitPos.x = (HitPos.x > EnemyPos.x) ? HitPos.x - 0.5f : HitPos.x + 0.5f;
			HitPos.y = (HitPos.y > EnemyPos.y) ? HitPos.y - 0.5f : HitPos.y + 0.5f;

			//Ž�� ��ġ �ֺ� ��� ã��
			NNInfoInternal nearestNodeInfo = gridGraph.GetNearest(HitPos, NNConstraint.None);
			//Ž���� ��� ����
			GraphNode nearestNode = nearestNodeInfo.node;
			//��� ��ġ Unity World Positionȭ
			Vector3 worldPosition = (Vector3)nearestNode.position;
			//���� ���� ����
			EndPoint = worldPosition;
		}
		else if (!HitObstacle)
		{
			//���� ���� ���� ��ġ���� ���� ���Ϳ� ���� �Ÿ���ŭ ���� ������ ����
			EndPoint = EnemyPos + (directionToTarget * DashDistance);
		}
		enemy.CheckForFacing(directionToTarget);
		var dashSeq = DOTween.Sequence();
		
		dashSeq.Append(enemy.transform.DOMove(EndPoint, DashTime).SetEase(Ease.OutCirc));

		dashSeq.Play()
		.OnUpdate(() =>
		{
			PlayerMain player;
			player = Physics2D.OverlapCircle(enemy.EnemyRB.position, 4, WhatIsEnemy).GetComponent<PlayerMain>();
			if (player != null)
			{
				player.GetDamage();
			}
		})
		.OnComplete(() =>
		{
			AttackCoroutine = null;
		});
	}

	private IEnumerator LockOnTarget()
	{
		yield return new WaitForSeconds(LockOnTime);
		TargetPos = enemy.Target.position;
	}
}
