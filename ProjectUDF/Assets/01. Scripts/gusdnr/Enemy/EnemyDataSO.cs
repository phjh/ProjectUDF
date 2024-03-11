using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	[HideInInspector] public float EnemyCurHP { get; set; }
	public float EnemyMaxHP;
	public float EnemyAttack;

	public float EnemyRoveRadius;

	public List<EnemyPatternBase> EnemyPatternList;

	public void ResetStat()
	{
		EnemyCurHP = EnemyMaxHP;
	}

	private void OnEnable()
	{
		ResetStat();
	}
}
