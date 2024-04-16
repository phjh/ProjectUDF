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
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
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
		//�÷��̾ �þ� ���� ���� ��
		if (enemy.IsWithStrikingDistance)
		{

			//���� ������ ������ ��
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				// ���� ������ ���·� ��ȯ
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
