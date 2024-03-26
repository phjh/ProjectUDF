using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : PoolableMono, IDamageable, IEnemyMoveable, ITriggerCheckable
{
	public float MaxHealth { get; set; }
	public float CurrentHealth { get; set; }
	public Rigidbody2D EnemyRB { get; set; }
	public bool IsFacingRight { get; set; } = true;


	public bool IsAggroed { get; set; }
	public bool IsWithStrikingDistance { get; set; }

	private bool isDead;

	#region State Machine Variables

	public EnemyStateMachine StateMachine { get; set; }

	public EnemyIdleState IdleState { get; set; }
	public EnemyChaseState ChaseState { get; set; }
	public EnemyAttackState AttackState { get; set; }

	#endregion

	#region Idle Variables
	public float RandomMovementRange = 5f;
	public float RandomMovementSpeed = 2f;
	#endregion

	private void OnEnable()
	{
		if(StateMachine == null)
		StateMachine = new EnemyStateMachine();

		if(IdleState == null)
		IdleState = new EnemyIdleState(this, StateMachine);
		if(ChaseState == null)
		ChaseState = new EnemyChaseState(this, StateMachine);
		if(AttackState == null)
		AttackState = new EnemyAttackState(this, StateMachine);
	}

	public override void ResetPooingItem()
	{
		CurrentHealth = MaxHealth;
		if(EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		isDead = false;
		StateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		StateMachine?.CurrentEnemyState.FrameUpdate();
	}

	private void FixedUpdate()
	{
		StateMachine?.CurrentEnemyState.PhtsicsUpdate();
	}

	#region Methods

	#region Manage Health/Die
	public void Damage(float damageAmount)
	{
		CurrentHealth -= damageAmount;
		if(CurrentHealth <= 0f) Die();
	}

	public void Die()
	{
		isDead = true;
	}
	#endregion

	#region Movement
	public void MoveEnemy(Vector2 moveVelocity)
	{
		EnemyRB.velocity = moveVelocity;
		CheckForFacing(moveVelocity);
	}

	public void CheckForFacing(Vector2 velocity)
	{
		if(IsFacingRight && velocity.x < 0f)
		{
			Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
			transform.rotation = Quaternion.Euler(rotator);
			IsFacingRight =!IsFacingRight;
		}
		else if(!IsFacingRight && velocity.x > 0f)
		{
			Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
			transform.rotation = Quaternion.Euler(rotator);
			IsFacingRight = !IsFacingRight;
		}
	}
	#endregion

	#region Animation Triggers
	private void AnimationTriggerEvent(AnimationTriggerType triggerType)
	{
		StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
	}

	public enum AnimationTriggerType
	{
		EnemyDamaged,
		EnemyMove
	}

	#endregion

	#region Distance Check
	public void SetAggroStatus(bool aggroStatus)
	{
		IsAggroed = aggroStatus;
	}

	public void SetStrikingDistance(bool isWithStrikingDistance)
	{
		IsWithStrikingDistance = isWithStrikingDistance;
	}
	#endregion

	#endregion
}