using Pathfinding;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;
	public float pathUpdateTime;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// 추가적인 초기화가 필요한 경우 여기서 설정
		clone.movementSpeed = movementSpeed;
		clone.pathUpdateTime = pathUpdateTime;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		enemy.EnemyAIPath.canMove = false;
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		enemy.EnemyAIPath.destination = enemy.Target.position;
		//플레이어가 시야 내에 있을 때
		if (enemy.IsWithStrikingDistance)
		{

			//공격 가능한 상태일 때
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				// 공격 가능한 상태로 전환
				//enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
				enemy.EnemyAIPath.canMove = true;
			}
		}
	}

	/*private IEnumerator SettingChaingPath()
	{
		do
		{
			if (enemy.EnemySeeker.IsDone())
				enemy.EnemySeeker.StartPath(enemy.transform.position, enemy.Target.position, OnPathComplete);
			yield return new WaitForSeconds(pathUpdateTime);
		}
		while (!reachedEndOfPath);
	}*/

	/*private void CheckingPath()
	{
		if (currentWayPoint >= ChasingPath.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 dircetion = (ChasingPath.vectorPath[currentWayPoint] - enemy.transform.position).normalized;
		Vector2 force = dircetion * movementSpeed;
		enemy.MoveEnemy(force);

		float distance = Vector2.Distance(enemy.transform.position, ChasingPath.vectorPath[currentWayPoint]);

		if (distance < nextWaypointDistance)
		{
			currentWayPoint = currentWayPoint + 1;
		}

	}*/

	/*private void OnPathComplete(Path pt)
	{
		if (!pt.error)
		{
			ChasingPath = pt;
			currentWayPoint = 0;
		}
	}*/

}
