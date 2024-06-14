using GameManageDefine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seeker))] //If Seeker Script is Null => Add Seeker
public class EnemyMain : PoolableMono
{
	public enum EnemyMotionState
	{
		None = 0,
		Move = 1,
		Attack = 2,
		Cooldown = 3,
		Dead = 4,
	}

	public enum CoolingState
	{
		None = 0,
		Start = 1,
		OnGoing = 2,
		End = 3
	}

	#region Enemy Variables
	public float MaxHealth { get; set; } //최대 체력
	public float CurrentHealth { get; set; } //현재 체력
	public bool IsFacingRight { get; set; } = true; //현재 보고 있는 방향이 오른쪽인가?
	public bool IsWithStrikingDistance { get; set; } //공격 감지 범위 안 쪽인가?
	public bool IsAttackCooldown { get; set; } //공격 쿨다운이 실행 중인가?
	public bool IsDead { get; set; } = false; //이 개체가 죽었는가?
	#endregion

	#region Enemy Components
	public Rigidbody2D EnemyRB { get; set; }
	public Transform Target { get; set; }
	public Transform MovePoint { get; set; }
	#endregion

	#region Enemy Pathfinding
	public Seeker ESeeker { get; set; }
	public Path EPath { get; set; }
	public int CurrentWaypoint { get; set; }
	#endregion

	#region State Machine Variables
	public EnemyStateMachine StateMachine { get; set; }

	[Header("EnemyMotionState Configuration")]
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
	#endregion

	#region Attack Variables
	[HideInInspector] public bool canAttack = true;
	#endregion

	private void Awake()
	{
		if(StateMachine == null)
		StateMachine = new EnemyStateMachine();

		ChaseState.Initialize(this, StateMachine);
		//Debug.Log("Start Cloning Chasing State");
		ChaseState = ChaseState.Clone();
		//if(ChaseState == null) Debug.Log("Chase state is Null");

		AttackState.Initialize(this, StateMachine);
		//Debug.Log("Start Cloning Attack State");
		AttackState = AttackState.Clone();
		//if(AttackState == null) Debug.LogError("Attack state is Null");

		CooldownState.Initialize(this, StateMachine);
		//Debug.Log("Start Cloning Cooldown State");
		CooldownState = CooldownState.Clone();
		//if(CooldownState == null) Debug.LogError("Cooldown State is Null");
	}

	private void Start()
	{
		ResetPoolingItem();
	}

	public override void ResetPoolingItem()
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
		Target = GameManager.Instance.player.transform;

		if(MovePoint == null) MovePoint = transform.Find("MovePoint").GetComponent<Transform>();
		MovePoint.gameObject.SetActive(true);

		if (EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		if (ESeeker == null) ESeeker = GetComponent<Seeker>();

		IsDead = false;
		canAttack = true;
		StateMachine.Initialize(ChaseState);

	}

	private void Update()
	{
		if (GameManager.Instance.gameState != GameStates.PauseUIOn)
		StateMachine.CurrentState?.FrameUpdate();
	}

	private void FixedUpdate()
	{
		if(GameManager.Instance.gameState != GameStates.PauseUIOn)
		StateMachine.CurrentState?.PhysicsUpdate();
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

	#region PathFinding

	public void UpdatePath()
	{
		if (ESeeker.IsDone()) ESeeker.StartPath(EnemyRB.position, Target.position, OnPathComplete);
	}

	private void OnPathComplete(Path pt)
	{
		if (!pt.error)
		{
			EPath = pt;
			CurrentWaypoint = 0;
		}
	}

	#endregion

	#region Manage Health/Die
	public void Damage(float damageAmount)
	{
		CurrentHealth -= damageAmount;
		if(CurrentHealth <= 0f)
		{
			SetDead();
		}
	}

	private void SetDead()
	{
		IsDead = true;
		MovePoint.gameObject.SetActive(false);
		OnDead();
	}

	public void OnDead()
	{
		StateMachine.CurrentState.ExitState();
		MapSystem.Instance.OnMonsterDead();
		StopAllCoroutines();
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
		if(IsFacingRight && velocity.x <= -0.01f)
		{
			Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
			transform.rotation = Quaternion.Euler(rotator);
			IsFacingRight =!IsFacingRight;
		}
		else if(!IsFacingRight && velocity.x >= 0.01f)
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, WhatIsObstacle);

		return !hit;
	}
	#endregion

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
 //       if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement move))
 //       {
 //           PlayerMain.Instance.GetDamage();
 //       }
 //   }

	#endregion
}
