using Pathfinding;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;
	public float pathUpdateTime;
	public float nextWaypointDistance;

	private Path path;
	private int currentWaypoint = 0;
	private bool reachedEndOfPath = false;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.name = name + "Clone";
		clone.movementSpeed = movementSpeed;
		clone.pathUpdateTime = pathUpdateTime;
		clone.nextWaypointDistance = nextWaypointDistance;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		CheckingValue();
		Debug.Assert(nameof(UpdatePath) == null, "updatepath null");
		enemy.InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateTime);
	}

	public void CheckingValue()
	{
		Debug.Assert(enemy == null, "Enemy is null");
	}

	public override void ExitState()
	{
		base.ExitState();
		enemy.CancelInvoke();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		
		//플레이어가 시야 내에 있을 때
		if (enemy.IsWithStrikingDistance)
		{
			//공격 가능한 상태일 때
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				// 공격 가능한 상태로 전환
				enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
			}
		}
		else
		{
			MoveToPath();
		}
	}

	private void MoveToPath()
	{
		if(path == null) return;

		if (currentWaypoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 dircetion = ((Vector2)path.vectorPath[currentWaypoint] - enemy.EnemyRB.position).normalized;
		Vector2 force = dircetion * movementSpeed;

		enemy.MoveEnemy(force);

		float distance = Vector2.Distance(enemy.EnemyRB.position, path.vectorPath[currentWaypoint]);

		if (distance < nextWaypointDistance)
		{
			currentWaypoint = currentWaypoint + 1;
		}
	}


	private void UpdatePath()
	{
		Debug.Log($"pos : {enemy.transform.position} rbpos : {enemy.EnemyRB.position}");
		if (enemy.ESeeker.IsDone()) enemy.ESeeker.StartPath(enemy.EnemyRB.position, enemy.Target.position, OnPathComplete);
	}


	private void OnPathComplete(Path pt)
	{
		if (!pt.error)
		{
			path = pt;
			currentWaypoint = 0;
		}
	}
}
