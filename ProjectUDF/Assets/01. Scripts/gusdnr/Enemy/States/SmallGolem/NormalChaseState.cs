using Pathfinding;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;
	public float pathUpdateTime;
	public float nextWaypointDistance;

	private Coroutine UpdatePathCoroutine;
	private Path ChasingPath;
	private int currentWayPoint = 0;
	private bool reachedEndOfPath = false;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.movementSpeed = movementSpeed;
		clone.pathUpdateTime = pathUpdateTime;
		clone.nextWaypointDistance = nextWaypointDistance;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
		currentWayPoint = 0;
		reachedEndOfPath = false;
		UpdatePathCoroutine = enemy.StartCoroutine(SettingChaingPath());
	}

	public override void ExitState()
	{
		base.ExitState();
		if (UpdatePathCoroutine != null)
		{
			enemy.StopCoroutine(UpdatePathCoroutine);
			UpdatePathCoroutine = null;
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		if(ChasingPath == null)
		{
			Debug.LogError($"A* Error! : {enemy.name}'s Chasing Path is null");
		}
		
		//�÷��̾ �þ� ���� ���� ��
		if (enemy.IsWithStrikingDistance)
		{
			//���� ������ ������ ��
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				// ���� ������ ���·� ��ȯ
				reachedEndOfPath = true;
				if(UpdatePathCoroutine != null)
				{
					enemy.StopCoroutine(UpdatePathCoroutine);
					UpdatePathCoroutine = null;
				}
				enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
			}
			else
			{
				CheckingPath();
			}
		}
		else
		{
			CheckingPath();
		}
	}

	private IEnumerator SettingChaingPath()
	{
		do
		{
			if (enemy.EnemySeeker.IsDone())
				enemy.EnemySeeker.StartPath(enemy.transform.position, enemy.Target.position, OnPathComplete);
			yield return new WaitForSeconds(pathUpdateTime);
		}
		while (!reachedEndOfPath);
	}

	private void CheckingPath()
	{
		if(currentWayPoint >= ChasingPath.vectorPath.Count)
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

		if(distance < nextWaypointDistance)
		{
			currentWayPoint = currentWayPoint + 1;
		}

	}	

	private void OnPathComplete(Path pt)
	{
		if (!pt.error)
		{
			ChasingPath = pt;
			currentWayPoint = 0;
		}
	}

}
