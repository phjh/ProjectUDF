using System.Collections;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;

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
	private bool isUpdatingPath = false;

	private bool hasLineOfSight = false;
	private bool isInAttackRange => Vector2.Distance(EnemyPos, TargetPos) < AttackDistance && hasLineOfSight;
	
	[HideInInspector] public bool isAttacking = false;
	#endregion

	#region Enemy Components
	public Transform Target; //���� �ڵ� �Ҵ��ϴ� ������� ���� ����

	[HideInInspector] public AIPath aiPath;
	[HideInInspector] public Collider2D EnemyCLD;
	[HideInInspector] public Rigidbody2D EnemyRB;

	private Seeker seeker;
	private EnemyBase eb;
	
	private IAttack DoingPattern;
	[Header("Attack Patterns")]
	public List<AtkPatternMono> Patterns = new List<AtkPatternMono>();
	#endregion

	#region Enemy Stats
	private float EnemyCurHP { get; set; }

	[Header("Attack Stats")]
	public float AttackDelay;
	public float AttackDistance;

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
		if (eb == null) eb = this;
		if (seeker == null) seeker = GetComponent<Seeker>();
		if (aiPath == null) aiPath = GetComponent<AIPath>();
		if (seeker.pathCallback == null) seeker.pathCallback += OnPathComplete;
		
		if(Patterns.Count < 0)
		{
			Patterns = GetComponents<AtkPatternMono>().ToList();
			Debug.Log("Add Attack Patterns In List");
		}
		DoingPattern = Patterns[0];
		
		isCanAttack = true;
		isDead = false;
		isWandering = false;
		isAttacking = false;
		isUpdatingPath = false;

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
			StopAllCoroutines();
			return;
		}
		
		if (!isAttacking && !isWandering)
		{
			if (isCanAttack && isInAttackRange)
			{
				ActiveAttack();
				/*
				�� ���� ���� ���൵ : ���� �Ǵ� �� ActiveAttack -> AtkPatternMono.DoingAttack(������ ����) -> DoingAttack ���� �� CooldownAttack ����
				*/
			}

			if (!isCanAttack && isInAttackRange)
			{
				StartCoroutine(WanderCoroutine());
			}

			if (!isInAttackRange && !isUpdatingPath)
			{
				StartCoroutine(UpdatePathCoroutine());
			}
		}
	}

	private void FixedUpdate()
	{
		//���� Ž�� �ڵ� (���� �����ϴ� ���� �����ϱ� ���ؼ�)
		RaycastHit2D ray = Physics2D.Raycast(EnemyPos, TargetPos - EnemyPos); //���� �÷��̾ ���ؼ� Ray�� ���ϴ�.
		if (ray.collider != null) hasLineOfSight = ray.collider.CompareTag("Player"); //���� Ray�� �÷��̾ �ƴ� ���� ���� �����ٸ� false��, �÷��̾��� true�� ��ȯ�մϴ�.
	}

	#region Methods

	public void GetDamage(float Damage)
	{
		EnemyCurHP -= Damage;
		if (EnemyCurHP < 0) isDead = true;
	}

	private void ActiveAttack()
	{
		aiPath.destination = EnemyPos;

		aiPath.isStopped = true;
		isAttacking = true;
		isCanAttack = false;

		StopCoroutine(UpdatePathCoroutine());
		DoingPattern.DoingAttack(eb);
	}

	private IEnumerator UpdatePathCoroutine()
	{
		Debug.Log("Update Path");
		isUpdatingPath = true;

		while (!isDead && isCanAttack && !isWandering && !isAttacking)
		{
			UpdatePath();
			yield return new WaitForSeconds(PathUpdateDelay);
		}

		isUpdatingPath = false;
	}

	private void UpdatePath() //��� ������Ʈ
	{
		if (seeker.IsDone() && !isDead && isCanAttack && !isWandering && !isAttacking)
			seeker.StartPath(EnemyPos, TargetPos, OnPathComplete);
	}

	private void OnPathComplete(Path p) //��� Ȯ��
	{
		if (!p.error && aiPath.reachedEndOfPath) aiPath.SetPath(p);
	}

	private IEnumerator WanderCoroutine()
	{
		Debug.Log("Wander Move");
		isWandering = true;
		aiPath.maxSpeed = WanderSpeed;
		// ���� �ð� ���� �÷��̾� �ֺ��� ��Ȳ
		float wanderTime = Random.Range(0.1f, 0.5f);
		Vector2 randomDirection = Random.insideUnitCircle.normalized * WanderRadius;
		Vector2 targetPosition = EnemyPos + randomDirection;

		aiPath.destination = targetPosition;

		yield return new WaitForSeconds(wanderTime);

		aiPath.maxSpeed = MovementSpeed;
		isWandering = false;
	}

	public IEnumerator CooldownAttack()
	{
		Debug.Log("Start Attack Cooldown");
		yield return new WaitForSeconds(AttackDelay);
		isCanAttack = true;
	}
	#endregion
}
