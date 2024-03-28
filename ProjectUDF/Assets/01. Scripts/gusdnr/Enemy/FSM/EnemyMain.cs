using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : PoolableMono
{
	#region Enemy Variables
	public float MaxHealth { get; set; }
	public float CurrentHealth { get; set; }
	public bool IsFacingRight { get; set; } = true;
	public bool IsWithStrikingDistance { get; set; }
	public bool IsAttackCooldown { get; set; }
	#endregion

	#region Enemy Components
	public Rigidbody2D EnemyRB { get; set; }
	public Transform Target { get; set; }
	public SpriteRenderer ERender { get; set; }
	#endregion

	#region State Machine Variables
	public EnemyStateMachine StateMachine { get; set; }

	[Header("State Configuration")]
	public EnemyState ChaseState;
	public EnemyState AttackState;
	public EnemyState CooldownState;
	#endregion

	#region Stat Variables
	[Header("Stats")]
	public float maxHealth = 30f;
	public bool isDead = false;
	public float attackCoolTime = 1f;
	#endregion

	#region Wander Variables
	[Header("Idle Variables")]
	public float RandomMovementRange = 5f;
	public float RandomMovementSpeed = 2f;
	public float MaxMoveTime = 4f;
	#endregion

	#region Chase Variables
	[Header("Chase Variables")]
	public float StrikingRadius = 3f;
	public float ChasingSpeed = 2.5f;
	#endregion

	#region Attack Variables
	[HideInInspector] public bool canAttack = true;
	#endregion

	private void Awake()
	{
		if(StateMachine == null)
		StateMachine = new EnemyStateMachine();

		SettingState(ChaseState);
		SettingState(CooldownState);
		SettingState(AttackState);
	}

	private void SettingState(EnemyState state)
	{
		state.Initialize(this, StateMachine);
		state = state.Clone();
		if (state == null)
			Debug.LogError("Null state detected.");
	}

	private void Start()
	{
		ResetPooingItem();
	}

	public override void ResetPooingItem()
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
		Target = GameManager.Instance.player.transform;
		if(EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		if(ERender == null) ERender = GetComponentInChildren<SpriteRenderer>();
		isDead = false;
		canAttack = true;
		StateMachine.Initialize(ChaseState, this);
	}

	private void Update()
	{
		StateMachine.CurrentState.FrameUpdate();
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentState.PhtsicsUpdate();
	}

	#region Methods

	public IEnumerator StartCooldown()
	{
		IsAttackCooldown = true;
		yield return new WaitForSeconds(attackCoolTime);
		IsAttackCooldown = false;
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
		StateMachine.CurrentState.ExitState();
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
		StateMachine.CurrentState.AnimationTriggerEvent(triggerType);
	}

	public enum AnimationTriggerType
	{
		EnemyDamaged,
		EnemyMove
	}

	#endregion

	#region Distance Check
	public void SetStrikingDistance(bool isWithStrikingDistance)
	{
		IsWithStrikingDistance = isWithStrikingDistance;
	}
	#endregion

	#endregion
}
