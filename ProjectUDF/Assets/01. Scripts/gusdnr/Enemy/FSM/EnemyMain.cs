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


	#region State Machine Variables

	public EnemyStateMachine StateMachine { get; set; }

	public EnemyIdleState IdleState { get; set; }
	public EnemyChaseState ChaseState { get; set; }
	public EnemyAttackState AttackState { get; set; }

	#endregion

	#region Stat Variables
	[Header("Stats")]
	public float maxHealth = 30f;
	public bool isDead = false;
	#endregion

	#region Idle Variables
	[Header("Idle Variables")]
	public float RandomMovementRange = 5f;
	public float RandomMovementSpeed = 2f;
	public float MaxMoveTime = 4f;
	#endregion

	#region Chase Variables
	[Header("Chase Variables")]
	public float AggroRadius = 7f;
	public float StrikingRadius = 3f;
	public float ChasingSpeed = 2.5f;
	#endregion

	#region Attack Variables
	[Header("Attack Variables")]
	public float AttackCoolTime = 3f;
	public bool canAttack = true;
	#endregion

	private void Awake()
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

	private void Start()
	{
		ResetPooingItem();
	}

	public override void ResetPooingItem()
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
		if(EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		isDead = false;
		canAttack = true;
		StateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		StateMachine.CurrentEnemyState.FrameUpdate();
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentEnemyState.PhtsicsUpdate();
	}

	#region Methods

	public IEnumerator StartAttackCooldown()
	{
		yield return new WaitForSeconds(AttackCoolTime);
		canAttack = true;
	}

	#region Manage Health/Die
	public void Damage(float damageAmount)
	{
		CurrentHealth -= damageAmount;
		if(CurrentHealth <= 0f) Die();
	}

	public void Die()
	{
		isDead = true;
		PoolManager.Instance.Push(this);
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
