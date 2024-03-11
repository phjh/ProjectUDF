using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : PoolableMono
{
	#region Enemy Base's Values
	public EnemyDataSO EnemyData;
	public bool isDead;
	public bool isCheckEnemy;
	public bool hasLineOfSight = false;
	#endregion

	public GameObject Player;

	#region Enemy Stats
	private float EnemyCurHP { get; set; }
	[HideInInspector] public float AttackDelay;
	[HideInInspector] public float MovementSpeed;
	[HideInInspector] public float SearchingRadius;
	[HideInInspector] public float RoveRadius;
	#endregion

	public EnemyPatternBase[] EnemyPatternList; //적이 사용할 패턴 목록

	private void OnEnable()
	{
		EnemyPatternList = GetComponents<EnemyPatternBase>();
	}

	public override void ResetPooingItem()
	{
		SettingStats();
		EnemyPatternList = GetComponents<EnemyPatternBase>();
	}

	private void SettingStats()
	{
		EnemyCurHP = EnemyData.EnemyMaxHP;
		AttackDelay = EnemyData.EnemyAttackDelay;
		MovementSpeed = EnemyData.EnemyMovementSpeed;
		SearchingRadius = EnemyData.EnemySearchingRadius;
		RoveRadius = EnemyData.EnemyRoveRadius;
	}

	public void GetDamage(float Damage)
	{
		EnemyCurHP -= Damage;
		if(EnemyCurHP < 0) isDead = true;
	}

	private void Update()
	{
		if(isDead)
		{
			PoolManager.Instance.Push(this.gameObject.GetComponent<EnemyBase>());
		}

		transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, MovementSpeed * Time.deltaTime);
	}

	private void FixedUpdate()
	{
		RaycastHit2D ray = Physics2D.Raycast(transform.position, Player.transform.position - transform.position);
		if(ray.collider != null)
		{
			hasLineOfSight = ray.collider.CompareTag("Player");
			if(hasLineOfSight)
			{
				Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.red);
			}
			else
			{
				Debug.DrawRay(transform.position, Player.transform.position - transform.position, Color.green);
			}
		}
	}
}
