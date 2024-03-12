using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPattern : MonoBehaviour
{
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashTime;
    [SerializeField] private float LockOnTime;

	EnemyPatternBrain PB;
	private Vector2 dir;
	private bool isLockOn;
	private bool isDash = false;

    public GameObject Player;

    void Update()
    {
		dir = PB.LastMovePos - PB.EnemyPos;

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
    }

	IEnumerator LockOnRoutine()
	{
		yield return new WaitForSeconds(LockOnTime);
		attackTarget = PB.LastMovePos;
		attackDir = attackTarget - PB.EnemyPos;
	}

	private void RayCast()
	{
		Debug.DrawRay(PB.EnemyRB.position, dir * 1.5f, new Color(0, 1, 0));
		RaycastHit2D rayHit = Physics2D.Raycast(PB.EnemyRB.position, dir, 1.5f);

		if (rayHit.collider != null)
		{
			scanGo = rayHit.collider.gameObject;
		}
		else scanGo = null;
	}

	private void Dash()
    {
		isAttacking = true;
		if (isAttacking)
		StartCoroutine(SetStateRoutine());
	}

	public float attackMoveSpeed;
	private GameObject scanGo;
	private Vector2 attackTarget;
	private Vector2 attackDir;
	private bool isAttacking;

	void FixedUpdate()
	{
		// Debug.Log(this.dist);

		if (this.dist < 4.0f && this.dist != 0)
		{
			RayCast();

			if (scanGo == null || scanGo.tag != "Obstacles")
			{
				if (!isLockOn)
				{
					isLockOn = true;
					this.speed = 0;
					StartCoroutine(LockOnRoutine());
					this.anim.SetInteger("State", 1);
				}
			}
		}

		if (isAttacking)
		{
			this.speed = tmpSpeed;
			this.rBody2D.velocity = attackDir.normalized * (this.speed * 3 - this.knockbackSpeed) * Time.deltaTime;
			
		}

		if (this.TargetInDistance() && this.followEnabled && !isAttacking) this.PathFollow();

		Debug.Log(isAttacking);
	}



	private	IEnumerator SetStateRoutine()
	{
		yield return new WaitForSeconds(DashTime);
		attackTarget = Vector2.zero;
		isAttacking = false;
		isLockOn = false;
	}

	
}
