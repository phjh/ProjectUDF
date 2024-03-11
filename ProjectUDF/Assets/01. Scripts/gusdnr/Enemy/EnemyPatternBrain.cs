using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatternBrain : MonoBehaviour
{
    private EnemyBase Base;

	public GameObject Player;
	private Vector2 EnemyPos;
	private Vector2 LastMovePos;
	private Vector2 PlayerPos;

	public bool hasLineOfSight = false;
	private bool isCanAttack = true;
	private bool isInAttackRange => Vector2.Distance(EnemyPos, PlayerPos) < Base.AttackDistance;


	private void Update()
	{
		EnemyPos = transform.position;
		PlayerPos = Player.transform.position;
		if (hasLineOfSight)
		{
			transform.position = Vector2.MoveTowards(EnemyPos, LastMovePos, Base.MovementSpeed * Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		RaycastHit2D ray = Physics2D.Raycast(EnemyPos, PlayerPos - EnemyPos);
		if (ray.collider != null)
		{
			hasLineOfSight = ray.collider.CompareTag("Player");
			if (hasLineOfSight)
			{
				LastMovePos = PlayerPos;
			}
		}
	}

	private void OnEnable()
	{
		Base = GetComponent<EnemyBase>();
	}

	private void JudgePattern()
	{
		if(hasLineOfSight && isInAttackRange)
		{
			if(isCanAttack)
			{
				//Active Attack Pattern
			}
			else
			{
				//Active Move Pattern
			}
		}
		else if (hasLineOfSight && !isInAttackRange)
		{
			//Active Move Pattern
		}
		else if (!hasLineOfSight)
		{
			//Active Rove Pattern or Idle
		}
	}
}
