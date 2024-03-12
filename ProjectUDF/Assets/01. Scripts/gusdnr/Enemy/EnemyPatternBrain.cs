using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatternBrain : MonoBehaviour
{
    private EnemyBase Base;

	public float MoveSpeed;

	public GameObject Player;
	public Vector2 EnemyPos;
	public Vector2 LastMovePos;
	public Vector2 PlayerPos;

	public Collider2D EnemyCLD;
	public Rigidbody2D EnemyRB;

	public bool hasLineOfSight = false;
	public bool isCanAttack = true;
	public bool isInAttackRange => Vector2.Distance(EnemyPos, PlayerPos) < Base.AttackDistance;


	private void Update()
	{
		EnemyPos = transform.position;
		PlayerPos = Player.transform.position;
		if (hasLineOfSight)
		{
			MoveSpeed = Base.MovementSpeed;
			transform.position = Vector2.MoveTowards(EnemyPos, LastMovePos, MoveSpeed * Time.deltaTime);
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
