using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPattern : MonoBehaviour
{
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashTime;
    [SerializeField] private float LockOnTime;

	EnemyPatternBrain PB;
	private Vector2 attackDir;
	private Vector2 attackTarget;
	private bool isLockOn;

    public GameObject Player;

    void Update()
    {
		if (PB.isCanAttack)
		{
			if (PB.isInAttackRange)
			{
				if (!isLockOn)
				{
					isLockOn = true;
					PB.MoveSpeed = 0;
					StartCoroutine(LockOnRoutine());
				}
			}
		}

		if (PB.isAttacking)
		{
			PB.EnemyRB.velocity = attackDir.normalized * (DashSpeed * Time.deltaTime);
			StartCoroutine(SetRoveState());
		}
	}

	private IEnumerator LockOnRoutine()
	{
		yield return new WaitForSeconds(LockOnTime);
		attackTarget = PB.LastMovePos;
		attackDir = attackTarget - PB.EnemyPos;
		PB.isAttacking = true;
	}

	private IEnumerator SetRoveState()
	{
		yield return new WaitForSeconds(DashTime);
		attackTarget = Vector2.zero;
		PB.isAttacking = false;
		isLockOn = false;
		PB.StartCoolDownAttack();
	}
}
