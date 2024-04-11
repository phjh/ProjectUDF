using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chase Chase", menuName = "SO/State/Chase/Normal")]
public class NormalChaseState : EnemyState
{
	public float movementSpeed;

	public override EnemyState Clone()
	{
		NormalChaseState clone = CloneBase() as NormalChaseState;
		// �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⼭ ����
		clone.movementSpeed = movementSpeed;
		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		//�÷��̾ �þ� ���� ���� ��
		if (enemy.IsWithStrikingDistance)
		{
			//���� ������ ������ ��
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				// ���� ������ ���·� ��ȯ
				enemy.MoveEnemy(Vector2.zero);
				enemy.StateMachine.ChangeState(enemy.AttackState);
			}
			else
				HandleObstacleDetection(); // ��ֹ� ���� �� �̵� ó��
		}
		else
		{
			HandleObstacleDetection();
		}
	}

	private void HandleObstacleDetection()
	{
		// ��ֹ� ����
		bool obstacleDetected = !enemy.UpdateFOV();

		// ��ֹ��� ������ ���
		if (obstacleDetected)
			MoveRandomly(); // ������ �������� ȸ���Ͽ� �̵�
		else
			MoveTowardsPlayer(); // ��ֹ��� ������ �÷��̾� �������� ����
	}

	private void MoveRandomly()
	{

		Vector2 closestDirection = (enemy.Target.position - enemy.MovePoint.position).normalized;
		float closestDistance = Vector2.Distance(enemy.MovePoint.position, enemy.Target.position);

		// ���� ���� ��ġ���� Ÿ�ٱ����� ���� ���� ���

		// Ÿ���� ���� ���� ���͸� ��������, �ð����� �ݽð�������� 90�� ȸ���� ���͸� ���
		Vector2 toTargetDirection = (enemy.Target.position - enemy.MovePoint.position).normalized;
		Vector2 rightAngleDirection = Quaternion.Euler(0, 0, -140) * toTargetDirection;
		Vector2 leftAngleDirection = Quaternion.Euler(0, 0, 140) * toTargetDirection;

		// �־��� ���� ���� ������ �ݺ��Ͽ� Ÿ�ٰ� �� ����� ������ ã���ϴ�.
		for (int i = 0; i < 15; i++) // 20�� �������� 14�� �ݺ��մϴ�.
		{
			// ���� �ݺ� �ε����� ���� �ð���� �Ǵ� �ݽð�������� ȸ���� ���� ���
			Vector2 rotatedDirection = (i % 2 == 0) ? Quaternion.Euler(0, 0, i * 10) * rightAngleDirection : Quaternion.Euler(0, 0, i * 10) * leftAngleDirection;
			Debug.Log("Searching Move Direction");
			// ȸ���� �������� ��ֹ��� �ִ��� �˻�
			RaycastHit2D hit = Physics2D.Raycast(enemy.MovePoint.position, rotatedDirection, 10f, enemy.WhatIsObstacle);
			if (hit.collider == null)
			{
				Debug.Log("No Obstacle");
				// ��ֹ��� ���� ���, Ÿ�ٰ��� �Ÿ��� ����Ͽ� �� ����� ������ �����մϴ�.
				float distanceToTarget = Vector2.Distance(enemy.MovePoint.position, enemy.Target.position);
				if (distanceToTarget < closestDistance)
				{
					closestDirection = rotatedDirection;
					closestDistance = distanceToTarget;
				}

				// ���� ���� ����� �������� �̵��մϴ�.
				Debug.Log($"Set Random Direction [{closestDirection.x} : {closestDirection.y}]");
				enemy.MoveEnemy(closestDirection * movementSpeed);
				Debug.DrawRay(enemy.MovePoint.position, closestDirection * movementSpeed, Color.red);
			}
			else if (hit.collider != null)
			{
				Debug.Log("Searching Obstacle");
				// ��ֹ��� �ִ� �������� �̵��մϴ�.
				Vector3 avoidDirection = Quaternion.Euler(0, 0, 90) * rotatedDirection.normalized;
				Vector2 avoidPosition = (enemy.MovePoint.position + avoidDirection) * 0.1f; // ��ֹ����� ���� �Ÿ��� ���� ��ġ
				enemy.MoveEnemy(avoidPosition);
				Debug.DrawRay(enemy.MovePoint.position, avoidDirection * movementSpeed, Color.blue);
			}
		}
	}

	private void MoveTowardsPlayer()
	{
		Debug.Log("Enter Move To Player");

		// �÷��̾� �������� ����
		Vector2 moveDirection = (enemy.Target.position - enemy.MovePoint.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);
		Debug.DrawRay(enemy.MovePoint.position, moveDirection * movementSpeed, Color.red);
	}

	
}
