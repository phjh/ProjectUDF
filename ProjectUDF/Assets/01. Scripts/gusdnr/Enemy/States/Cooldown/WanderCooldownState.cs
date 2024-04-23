using DG.Tweening;
using Pathfinding;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown State", menuName = "SO/State/Cooldown/Wander")]
public class WanderCooldownState : EnemyState
{
	public float MinDistance = 1f;
	public float MaxDistance = 3f;
	public float MoveTime = 5f;

	public LayerMask WhatIsObstacle;

	private GridGraph gridGraph;

	private Vector2 EnemyPos;
	private Vector2 EndPoint;
	private Vector2 targetDirection;

	public override EnemyState Clone()
	{
		WanderCooldownState clone = (WanderCooldownState)CloneBase();
		clone.MinDistance = MinDistance;
		clone.MaxDistance = MaxDistance;
		clone.MoveTime = MoveTime;
		clone.WhatIsObstacle = WhatIsObstacle;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		DOTween.Init();
		enemy.MoveEnemy(Vector2.zero);

		gridGraph = AstarPath.active.data.gridGraph;
		SetNewDestination();
		enemy.StartCoroutine(enemy.StartCooldown());
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.MoveEnemy(Vector2.zero);
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		EnemyPos = enemy.EnemyRB.position;
		if (enemy.IsAttackCooldown == false)
		{
			enemy.DOKill();
			enemy.StateMachine.ChangeState(enemy.ChaseState);
		}
	}

	private void SetNewDestination()
	{
		// ������ ���� ����
		Vector2 randomDirection = Random.insideUnitCircle.normalized;
		Vector2 moveDIr = (randomDirection - EnemyPos).normalized;
		float distance = Random.Range(MinDistance, MaxDistance);
		// ������ �Ÿ� ����
		RaycastHit2D HitObstacle = Physics2D.Raycast(enemy.EnemyRB.position, moveDIr, distance, WhatIsObstacle);
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
			EndPoint = EnemyPos + (moveDIr * distance);
		}
		enemy.CheckForFacing(moveDIr);

		var wanderSeq = DOTween.Sequence();

		wanderSeq.Append(enemy.transform.DOMove(EndPoint, MoveTime).SetEase(Ease.InQuad));

		wanderSeq.Play().OnComplete(() =>
		{
			if (enemy.IsAttackCooldown == true)
			{
				SetNewDestination();
			}
			if (enemy.IsAttackCooldown == false)
			{
				enemy.StateMachine.ChangeState(enemy.ChaseState);
			}
		});
	}
}
