using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
			MoveTowardsPlayer();
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
		Vector2 closestDirection; 
		Vector2 toTargetDirection;
		closestDirection = toTargetDirection = (enemy.Target.position - enemy.transform.position).normalized;
		float closestDistance = Vector2.Distance(enemy.transform.position, enemy.Target.position);

		// ���� ���� ��ġ���� Ÿ�ٱ����� ���� ���� ���

		// Ÿ���� ���� ���� ���͸� ��������, �ð����� �ݽð�������� 90�� ȸ���� ���͸� ���
		Vector2 rightAngleDirection = Quaternion.Euler(0, 0, -90) * toTargetDirection;
		Vector2 leftAngleDirection = Quaternion.Euler(0, 0, 90) * toTargetDirection;

		// �־��� ���� ���� ������ �ݺ��Ͽ� Ÿ�ٰ� �� ����� ������ ã���ϴ�.
		for (int i = 0; i < 18; i++) // 20�� �������� 18�� �ݺ��մϴ�.
		{
			// ���� �ݺ� �ε����� ���� �ð���� �Ǵ� �ݽð�������� ȸ���� ���� ���
			Vector2 rotatedDirection = (i % 2 == 0) ? Quaternion.Euler(0, 0, i * 10) * rightAngleDirection : Quaternion.Euler(0, 0, i * 10) * leftAngleDirection;

			// ȸ���� �������� ��ֹ��� �ִ��� �˻�
			RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, rotatedDirection, 10f, enemy.WhatIsObstacle);
			if (hit.collider == null)
			{
				// ��ֹ��� ���� ���, Ÿ�ٰ��� �Ÿ��� ����Ͽ� �� ����� ������ �����մϴ�.
				float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.Target.position);
				if (distanceToTarget < closestDistance)
				{
					closestDirection = rotatedDirection;
					closestDistance = distanceToTarget;
				}
			}
		}
		// ���� ���� ����� �������� �̵��մϴ�.
		enemy.MoveEnemy(closestDirection * movementSpeed);
	}

	private void MoveTowardsPlayer()
	{
		// �÷��̾� �������� ����
		Vector2 moveDirection = (enemy.Target.position - enemy.transform.position).normalized;
		enemy.MoveEnemy(moveDirection * movementSpeed);
	}


}
