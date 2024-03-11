using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : PoolableMono
{
	#region Enemy Base's Values
	public EnemyDataSO EnemyData;
	public bool isDead;

	public Vector2 MovePosition;
	#endregion

	#region Enemy Stats
	private float EnemyCurHP { get; set; }
	[HideInInspector] public float AttackDelay;
	[HideInInspector] public float MovementSpeed;
	[HideInInspector] public float SearchingRadius;
	[HideInInspector] public float RoveRadius;
	[HideInInspector] public float AttackDistance;
	#endregion

	public IPatternBase[] EnemyPatternList; //적이 사용할 패턴 목록

	private void OnEnable()
	{
		EnemyPatternList = GetComponents<IPatternBase>();
		SettingStats();
	}

	public override void ResetPooingItem()
	{
		SettingStats();
		EnemyPatternList = GetComponents<IPatternBase>();
	}

	private void Update()
	{
		if(isDead)
		{
			PoolManager.Instance.Push(this.gameObject.GetComponent<EnemyBase>());
		}
		
	}

	#region Methods
	private void SettingStats()
	{
		EnemyCurHP = EnemyData.EnemyMaxHP;
		AttackDelay = EnemyData.EnemyAttackDelay;
		MovementSpeed = EnemyData.EnemyMovementSpeed;
		SearchingRadius = EnemyData.EnemySearchingRadius;
		RoveRadius = EnemyData.EnemyRoveRadius;
		AttackDistance = EnemyData.EnemyAttackDistance;
	}

	public void GetDamage(float Damage)
	{
		EnemyCurHP -= Damage;
		if (EnemyCurHP < 0) isDead = true;
	}
	#endregion
}
