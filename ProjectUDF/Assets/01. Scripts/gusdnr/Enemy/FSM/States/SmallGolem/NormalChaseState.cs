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
			Debug.Log("Enemy In AttackRange");
			//���� ������ ������ ��
			if (enemy.UpdateFOV() && !enemy.IsAttackCooldown)
			{
				Debug.Log("Enemy Do Attack");
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

		Debug.Log("Enemy Do Search");
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

		Debug.Log("Enter Move Random");

		Vector2 closestDirection = (enemy.Target.position - enemy.transform.position).normalized; ; 
		float closestDistance = Vector2.Distance(enemy.transform.position, enemy.Target.position);

		// ���� ���� ��ġ���� Ÿ�ٱ����� ���� ���� ���

		// Ÿ���� ���� ���� ���͸� ��������, �ð����� �ݽð�������� 90�� ȸ���� ���͸� ���
		Vector2 toTargetDirection = (enemy.Target.position - enemy.transform.position).normalized; ;
		Vector2 rightAngleDirection = Quaternion.Euler(0, 0, -90) * toTargetDirection;
		Vector2 leftAngleDirection = Quaternion.Euler(0, 0, 90) * toTargetDirection;

		// �־��� ���� ���� ������ �ݺ��Ͽ� Ÿ�ٰ� �� ����� ������ ã���ϴ�.
		for (int i = 0; i < 9; i++) // 40�� �������� 9�� �ݺ��մϴ�.
		{
			// ���� �ݺ� �ε����� ���� �ð���� �Ǵ� �ݽð�������� ȸ���� ���� ���
			Vector2 rotatedDirection = (i % 2 == 0) ? Quaternion.Euler(0, 0, i * 20) * rightAngleDirection : Quaternion.Euler(0, 0, i * 20) * leftAngleDirection;
			Debug.Log("Searching Move Direction");
			// ȸ���� �������� ��ֹ��� �ִ��� �˻�
			RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, rotatedDirection, 10f, enemy.WhatIsObstacle);
			if (hit.collider == null)
			{
				Debug.Log("No Obstacle");
				// ��ֹ��� ���� ���, Ÿ�ٰ��� �Ÿ��� ����Ͽ� �� ����� ������ �����մϴ�.
				float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.Target.position);
				if (distanceToTarget < closestDistance)
				{
					closestDirection = rotatedDirection;
					closestDistance = distanceToTarget;
				}
			}
			else if (hit.collider != null)
			{
				Debug.Log("Searching Obstacle");
				closestDirection = rotatedDirection;
				break;
			}
		}
		// ���� ���� ����� �������� �̵��մϴ�.
		Debug.Log($"Set Random Direction [{closestDirection.x} : {closestDirection.y}]");
		enemy.MoveEnemy(closestDirection * movementSpeed);
	}

	private void MoveTowardsPlayer()
	{
		Debug.Log("Enter Move To Player");

		// �÷��̾� �������� ����
		Vector2 moveDirection = (enemy.Target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);
	}


}
