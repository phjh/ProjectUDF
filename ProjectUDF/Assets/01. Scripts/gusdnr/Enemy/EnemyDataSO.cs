using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	[Range(1, 5000)]public float EnemyMaxHP; //�� �ִ� HP
	[Range(0, 100)]public float EnemyMovementSpeed; //�� �̵� �ӵ�
	[Range(0, 120)]public float EnemyAttackDelay; //�� ���� �� ������

	[Range(0, 50)]public float EnemySearchingRadius; //�� �÷��̾� Ž�� ����
	[Range(0, 50)]public float EnemyAttackDistance; //�� ���� ���� ����
	[Range(0, 5)] public float EnemyRoveRadius; //�� ��Ȳ �� ���� ��ġ �Ҵ� ����
}
