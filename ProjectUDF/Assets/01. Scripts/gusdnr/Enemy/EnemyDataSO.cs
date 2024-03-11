using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	public float EnemyMaxHP; //적 최대 HP
	public float EnemyMovementSpeed; //적 이동 속도
	public float EnemyAttackDelay; //적 공격 간 딜레이

	public float EnemySearchingRadius; //적 플레이어 탐색 범위
	public float EnemyRoveRadius; //적 방황 시 다음 위치 할당 범위
}
