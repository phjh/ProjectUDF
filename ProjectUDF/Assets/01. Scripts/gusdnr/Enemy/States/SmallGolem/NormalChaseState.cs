using Pathfinding;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;
	public float pathUpdateTime;
	public float nextWaypointDistance;

	private bool reachedEndOfPath = false;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.movementSpeed = movementSpeed;
		clone.pathUpdateTime = pathUpdateTime;
		clone.nextWaypointDistance = nextWaypointDistance;

		clone.reachedEndOfPath = false;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.UpdatePath();
		enemy.InvokeRepeating(nameof(enemy.UpdatePath), 0f, pathUpdateTime);
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.CancelInvoke();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		
		//�÷��̾ �þ� ���� ���� ��
		if (enemy.IsWithStrikingDistance)
		{
			//���� ������ ������ ��
			if (enemy.UpdateFOV() == true && !enemy.IsAttackCooldown)
			{
				// ���� ������ ���·� ��ȯ
				enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
			}
			else
			{
				MoveToPath();
			}
		}
		else
		{
			MoveToPath();
		}
	}

	private void MoveToPath()
	{
		if(enemy.EPath == null)
		{
			return;
		}

		if (enemy.CurrentWaypoint >= enemy.EPath.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 dircetion = ((Vector2)enemy.EPath.vectorPath[enemy.CurrentWaypoint] - enemy.EnemyRB.position).normalized;
		Vector2 force = dircetion * movementSpeed;

		enemy.MoveEnemy(force);

		float distance = Vector2.Distance(enemy.EnemyRB.position, enemy.EPath.vectorPath[enemy.CurrentWaypoint]);

		if (distance < nextWaypointDistance)
		{
			enemy.CurrentWaypoint = enemy.CurrentWaypoint + 1;
		}
	}

}
