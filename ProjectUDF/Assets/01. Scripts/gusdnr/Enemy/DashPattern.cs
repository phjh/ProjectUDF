using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPattern : MonoBehaviour
{
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashDistance;

    private bool isDash = false;

    public GameObject Player;

    void Update()
    {
        
    }

    private void Dash()
    {
        
    }

	public float attackMoveSpeed;
	private GameObject scanGo;
	private Vector2 attackTarget;
	private Vector2 attackDir;
	private bool isLockOn;
	private bool isAttacking;


	protected override void Start()
	{
		base.Start();
	}

	void FixedUpdate()
	{
		// Debug.Log(this.dist);

		if (this.dist < 4.0f && this.dist != 0)
		{
			this.RayCast();

			if (this.scanGo == null || this.scanGo.tag != "Obstacles")
			{
				if (!this.isLockOn)
				{
					this.isLockOn = true;
					this.speed = 0;
					StartCoroutine(LockOnRoutine());
					this.anim.SetInteger("State", 1);
				}
			}
		}

		if (this.isAttacking)
		{
			this.speed = tmpSpeed;
			this.rBody2D.velocity = this.attackDir.normalized * (this.speed * 3 - this.knockbackSpeed) * Time.deltaTime;
			StartCoroutine(SetStateRoutine());
		}

		if (this.TargetInDistance() && this.followEnabled && !this.isAttacking) this.PathFollow();

		Debug.Log(this.isAttacking);
	}

	private void DashAttack()
	{
		this.isAttacking = true;
	}

	IEnumerator LockOnRoutine()
	{
		yield return new WaitForSeconds(0.8f);
		this.attackTarget = this.target.transform.position;
		this.attackDir = this.attackTarget - (Vector2)this.transform.position;
	}

	IEnumerator SetStateRoutine()
	{
		yield return new WaitForSeconds(0.5f);

		this.speed = tmpSpeed;
		this.attackTarget = Vector2.zero;
		this.anim.SetInteger("State", 0);
		this.speed = this.tmpSpeed;
		this.isAttacking = false;
		this.isLockOn = false;
	}

	private void RayCast()
	{
		Debug.DrawRay(this.rBody2D.position, this.dir * 1.5f, new Color(0, 1, 0));
		RaycastHit2D rayHit = Physics2D.Raycast(this.rBody2D.position, this.dir, 1.5f);

		if (rayHit.collider != null)
		{
			this.scanGo = rayHit.collider.gameObject;
		}
		else this.scanGo = null;
	}
}
