using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	[Range(1, 5000)]public float EnemyMaxHP; //적 최대 HP
	[Range(0, 100)]public float EnemyMovementSpeed; //적 이동 속도
	[Range(0, 120)]public float EnemyAttackDelay; //적 공격 간 딜레이

	[Range(0, 50)]public float EnemySearchingRadius; //적 플레이어 탐색 범위
	[Range(0, 50)]public float EnemyAttackDistance; //적 공격 실행 범위
	[Range(0, 5)] public float EnemyRoveRadius; //적 방황 시 다음 위치 할당 범위
}
