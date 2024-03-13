using System.Collections;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class EnemyBase : PoolableMono
{
	#region Enemy Base's Values
	private Vector2 RandomPos;
	[HideInInspector] public Vector2 TargetPos;
	[HideInInspector] public Vector2 EnemyPos;
	[HideInInspector] public Vector2 LastMovePos;

	private bool isDead;
	private bool isCanAttack = true;
	private bool isWandering = false;
	
	private bool hasLineOfSight = false;
	private bool isInAttackRange => Vector2.Distance(EnemyPos, TargetPos) < AttackDistance && hasLineOfSight;
	
	[HideInInspector] public bool isAttacking = false;
	#endregion

	#region Enemy Components
	public Transform Target; //���� �ڵ� �Ҵ��ϴ� ������� ���� ����

	[HideInInspector] public Collider2D EnemyCLD;
	[HideInInspector] public Rigidbody2D EnemyRB;

	private Seeker seeker;
	private AIPath aiPath;
	
	private IAttack DoingPattern;
	[Header("Attack Patterns")]
	public List<IAttack> Patterns = new List<IAttack>();
	#endregion

	#region Enemy Stats
	private float EnemyCurHP { get; set; }

	[Header("Attack Stats")]
	public float AttackDelay;
	public float AttackDistance;
	public float CurDistance;

	[Header("Move Stats")]
	public float PathUpdateDelay;
	public float MovementSpeed;
	public float WanderSpeed;
	public float WanderRadius;
	#endregion

	private void Start()
	{
		ResetPooingItem();
	}

	public override void ResetPooingItem()
	{
		if (EnemyCLD == null) EnemyCLD = GetComponent<Collider2D>();
		if (EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		if (seeker == null) seeker = GetComponent<Seeker>();
		if (aiPath == null) aiPath = GetComponent<AIPath>();
		if (seeker.pathCallback == null) seeker.pathCallback += OnPathComplete;

		DoingPattern = Patterns[0];
		
		isCanAttack = true;
		isDead = false;
		isWandering = false;
		isAttacking = false;

		aiPath.maxSpeed = MovementSpeed;
	}

	private void Update()
	{
		EnemyPos = transform.position;
		TargetPos = Target.position;

		if (isDead)
		{
			//���� ���� ���̶�� ����ϴ� �κ� �߰�
			PoolManager.Instance.Push(this.gameObject.GetComponent<EnemyBase>());
			return;
		}
		else if (!isDead)
		{
			if (isCanAttack && isInAttackRange && !isWandering)
			{
				if (!isAttacking) StartCoroutine(AttackCoroutine());
				
			}

			if (!isCanAttack && isInAttackRange)
			{
				if (!isWandering) StartCoroutine(WanderCoroutine());
			}

			if (!isInAttackRange)
			{
				InvokeRepeating(nameof(UpdatePath), 0f, PathUpdateDelay);
			}
		}
	}

	private void FixedUpdate()
	{
		RaycastHit2D ray = Physics2D.Raycast(EnemyPos, TargetPos - EnemyPos);
		if (ray.collider != null) hasLineOfSight = ray.collider.CompareTag("Player");
	}

	#region Methods

	public void GetDamage(float Damage)
	{
		EnemyCurHP -= Damage;
		if (EnemyCurHP < 0) isDead = true;
	}

	private IEnumerator AttackCoroutine()
	{
		isAttacking = true;
		DoingPattern.DoingAttack(this);
		yield return new WaitForSeconds(AttackDelay);
	}

	private void UpdatePath() //��� ������Ʈ
	{
		if (seeker.IsDone() && !isDead && isCanAttack && !isWandering)
			seeker.StartPath(EnemyPos, TargetPos, OnPathComplete);
	}

	private void OnPathComplete(Path p) //��� Ȯ��
	{
		if (!p.error && aiPath.reachedEndOfPath) aiPath.SetPath(p);
	}

	private IEnumerator WanderCoroutine()
	{
		isWandering = true;
		aiPath.maxSpeed = WanderSpeed;
		// ���� �ð� ���� �÷��̾� �ֺ��� ��Ȳ
		float wanderTime = Random.Range(1f, 5f);
		Vector2 randomDirection = Random.insideUnitCircle.normalized * WanderRadius;
		Vector2 targetPosition = EnemyPos + randomDirection;

		aiPath.destination = targetPosition;

		yield return new WaitForSeconds(wanderTime);

		aiPath.maxSpeed = MovementSpeed;
		isWandering = false;
	}

	#endregion
}