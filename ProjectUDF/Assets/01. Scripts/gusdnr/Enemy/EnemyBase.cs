using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyBase : PoolableMono
{
	#region Enemy Base's Values
	private Vector2 RandomPos;
	[HideInInspector] public Vector2 EnemyPos;
	[HideInInspector] public Vector2 LastMovePos;
	[HideInInspector] public Vector2 PlayerPos;

	private bool isDead;
	private bool isCanAttack = true;
	private bool isWandering = false;
	
	[HideInInspector] public bool isAttacking = false;
	[HideInInspector] public bool isInAttackRange => Vector2.Distance(EnemyPos, PlayerPos) < AttackDistance;
	#endregion

	#region Enemy Components
	public Transform Target; //���� �ڵ� �Ҵ��ϴ� ������� ���� ����

	[HideInInspector] public Collider2D EnemyCLD;
	[HideInInspector] public Rigidbody2D EnemyRB;

	private Seeker seeker;
	private AIPath aiPath;
	
	private IAttack DoingPattern;
	[Header("Attack Patterns")]
	public IAttack[] Patterns;
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
	}

	private void Update()
	{
		if (isDead)
		{
			//���� ���� ���̶�� ����ϴ� �κ� �߰�
			PoolManager.Instance.Push(this.gameObject.GetComponent<EnemyBase>());
			return;
		}
		else if (!isDead)
		{
			if (isCanAttack && isInAttackRange)
			{
				if (!isAttacking)
				{
					isWandering = false;
					StartCoroutine(AttackCoroutine());
				}
			}
			else if (!isCanAttack && isInAttackRange)
			{
				if (!isWandering)
				{
					StartCoroutine(WanderCoroutine());
				}
			}
			else if (!isInAttackRange)
			{
				InvokeRepeating(nameof(UpdatePath), 0f, PathUpdateDelay);
			}
		}
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
			seeker.StartPath(transform.position, Target.position, OnPathComplete);
	}

	private void OnPathComplete(Path p) //��� Ȯ��
	{
		if (!p.error) aiPath.SetPath(p);
	}

	private IEnumerator WanderCoroutine()
	{
		isWandering = true;
		aiPath.maxSpeed = WanderSpeed;
		// ���� �ð� ���� �÷��̾� �ֺ��� ��Ȳ
		float wanderTime = Random.Range(1f, 5f);
		Vector2 randomDirection = Random.insideUnitCircle.normalized * WanderRadius;
		Vector2 targetPosition = (Vector2)transform.position + randomDirection;

		aiPath.destination = targetPosition;

		yield return new WaitForSeconds(wanderTime);

		aiPath.maxSpeed = MovementSpeed;
		isWandering = false;
	}

	#endregion
}
