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
	public LayerMask WhatIsObstacle;
	public LayerMask WhatIsPlayer;
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
		ResetPooingItem();

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

	public override void ResetPooingItem()
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
		if (GameManager.Instance.gameState != GameStates.PauseUIOn)
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

	public bool UpdateFOV()
	{
		Vector2 direction = Target.position - transform.position;
		float rayDistance = Vector2.Distance(Target.position, transform.position);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, WhatIsObstacle | WhatIsPlayer);
		if (hit.collider != null)
		{
			if (hit.collider.CompareTag("Player")) return true; // 대상이 플레이어인 경우
			else return false; // 대상이 장애물인 경우
		}
		else return false; // 레이가 아무 것도 충돌하지 않은 경우
	}
	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player playerMain = collision.GetComponent<Player>();
			if(playerMain != null) Debug.Log("Hit : Player Is Not Null");
		}
	}

	#endregion
}
