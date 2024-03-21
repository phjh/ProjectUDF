using System.Collections;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class EnemyBase : PoolableMono
{
	#region Enemy Base's Values
	[HideInInspector] public Vector2 TargetPos;
	[HideInInspector] public Vector2 EnemyPos;

	private bool isDead;
	private bool isCanAttack = true;
	private bool isWandering = false;
	private bool isUpdatingPath = false;

	private bool hasLineOfSight = false;
	private bool isInAttackRange => Vector2.Distance(EnemyPos, TargetPos) < MaxAttackRadius && Vector2.Distance(EnemyPos, TargetPos) > MinAttackRadius && hasLineOfSight;
	
	[HideInInspector] public bool isAttacking = false;
	#endregion

	#region Enemy Components
	public Transform target; //���� �ڵ� �Ҵ��ϴ� ������� ���� ����

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

	[Header("Normal Stats")]
	public float MaxHP;

	[Header("Attack Stats")]
	public float AttackDelay;
	public float MinAttackRadius;
	public float MaxAttackRadius;

	[Header("Move Stats")]
	public float PathUpdateDelay;
	public float MovementSpeed;
	public float WanderRadius;
	#endregion

	private void Start()
	{
		ResetPooingItem();
	}

	public override void ResetPooingItem()
	{
		#region Null Check when Reset
		if (EnemyCLD == null) EnemyCLD = GetComponent<Collider2D>();
		if (EnemyRB == null) EnemyRB = GetComponent<Rigidbody2D>();
		if (eb == null) eb = this;
		if (seeker == null) seeker = GetComponent<Seeker>();
		if (aiPath == null) aiPath = GetComponent<AIPath>();
		if (seeker.pathCallback == null) seeker.pathCallback += OnPathComplete;
		if(target == null) target = GameManager.Instance.player.transform;
		#endregion

		if (Patterns.Count < 0) //���� ������ 0������ ���� �� ���� ����Ʈ �Ҵ�
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
		
		EnemyCurHP = MaxHP;
		aiPath.maxSpeed = MovementSpeed;
	}

	private void Update()
	{
		EnemyPos = transform.position;
		TargetPos = target.position;

		if (isDead)
		{
			StopAllCoroutines();
			PoolManager.Instance.Push(this.gameObject.GetComponent<EnemyBase>());
		}
		
		if (!isAttacking && !isWandering)
		{
			if (isCanAttack && isInAttackRange)
			{
				aiPath.destination = EnemyPos;
				StopCoroutine(UpdatePathCoroutine());
				StopCoroutine(WanderCoroutine());
				ActiveAttack();
				/*
				�� ���� ���� ���൵ : ���� �Ǵ� �� ActiveAttack -> AtkPatternMono.DoingAttack(������ ����) -> DoingAttack ���� �� CooldownAttack ����
				*/
			}
			else if (!isCanAttack && isInAttackRange)
			{
				StopCoroutine(UpdatePathCoroutine());
				StartCoroutine(WanderCoroutine());
			}
			else if (!isUpdatingPath)
			{
				StopCoroutine(WanderCoroutine());
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
		aiPath.isStopped = true;
		isAttacking = true;
		isCanAttack = false;

		if (DoingPattern != null) DoingPattern.DoingAttack(eb);
	}

	private IEnumerator UpdatePathCoroutine()
	{
		if (isUpdatingPath)	yield break;

		Debug.Log("Update Path");
		isUpdatingPath = true;

		while (!isDead && !isWandering && !isAttacking)
		{
			if(!isAttacking) UpdatePath();
			yield return new WaitForSeconds(PathUpdateDelay);
			if(isAttacking) break;
		}

		isUpdatingPath = false;
	}

	private void UpdatePath() //��� ������Ʈ
	{
		if(seeker.IsDone() && !isDead && !isAttacking)
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
		// ���� �ð� ���� �÷��̾� �ֺ��� ��Ȳ
		float wanderTime = Random.Range(0.5f, 2f);
		Vector2 randomDirection = Random.insideUnitCircle.normalized * WanderRadius;
		Vector2 targetPosition = EnemyPos + randomDirection;

		aiPath.destination = targetPosition;

		yield return new WaitForSeconds(wanderTime);

		isWandering = false;
	}

	public IEnumerator CooldownAttack()
	{
		Debug.Log("Start Attack Cooldown");
		aiPath.maxSpeed = MovementSpeed;
		yield return new WaitForSeconds(AttackDelay);
		isCanAttack = true;
	}
	#endregion

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, MinAttackRadius);
		Gizmos.DrawWireSphere(transform.position, MaxAttackRadius);
	}


}
