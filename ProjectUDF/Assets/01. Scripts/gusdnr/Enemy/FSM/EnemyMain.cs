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
	public bool IsDead { get; set; } = false;
	#endregion

	#region Enemy Components
	public Rigidbody2D EnemyRB { get; set; }
	public Transform Target { get; set; }
	public SpriteRenderer EnemySR { get; set; }
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
	public float attackCoolTime = 1f;
	#endregion

	#region Chase Variables
	[Header("Chase Variables")]
	public float StrikingRadius = 3f;
	#endregion

	#region Attack Variables
	[HideInInspector] public bool canAttack = true;
	#endregion

	private void Awake()
	{
		if(StateMachine == null)
		StateMachine = new EnemyStateMachine();
	}

	private void Start()
	{
		ResetPoolingItem();

		ChaseState.Initialize(this, StateMachine);
		ChaseState = ChaseState.Clone();
		if (ChaseState == null) Debug.LogError("Chase state is Null");

		AttackState.Initialize(this, StateMachine);
		AttackState = AttackState.Clone();
		if (AttackState == null) Debug.LogError("Attack state is Null");

		CooldownState.Initialize(this, StateMachine);
		CooldownState = CooldownState.Clone();
		if (CooldownState == null) Debug.LogError("Cooldown state is Null");
	}

	public override void ResetPoolingItem()
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
		Target = GameManager.Instance.player.transform;
		if(EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		if(EnemySR == null) EnemySR = GetComponentInChildren<SpriteRenderer>();
		IsDead = false;
		canAttack = true;
		StateMachine.Initialize(ChaseState);
	}

	private void Update()
	{
		if(GameManager.Instance.gameState != GameStates.PauseUIOn)
		StateMachine.CurrentState.FrameUpdate();
	}

	private void FixedUpdate()
	{
		if(GameManager.Instance.gameState != GameStates.PauseUIOn)
		StateMachine.CurrentState.PhtsicsUpdate();
	}

	#region Methods

	public IEnumerator StartCooldown()
	{
		IsAttackCooldown = true;
		float curCoolTime = 0;
		while(curCoolTime <= attackCoolTime)
		{
			if (GameManager.Instance.gameState != GameStates.PauseUIOn)
			{
				curCoolTime += Time.deltaTime;
			}
			yield return null;
		}
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
		IsDead = true;
		StateMachine.CurrentState.ExitState();
		MapSystem.Instance.ActionInvoker(MapEvents.MonsterKill);
		PoolManager.Instance.Push(this, pair.enumtype);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == GameManager.Instance.player.gameObject)
		{
			PlayerMovement move = collision.gameObject.GetComponent<PlayerMovement>();
			//move.GetDamage();
		}
    }

    #endregion
}
