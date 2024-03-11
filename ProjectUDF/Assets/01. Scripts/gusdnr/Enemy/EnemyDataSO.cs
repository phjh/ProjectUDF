using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
	public float EnemyMaxHP; //�� �ִ� HP
	public float EnemyMovementSpeed; //�� �̵� �ӵ�
	public float EnemyAttackDelay; //�� ���� �� ������

	public float EnemySearchingRadius; //�� �÷��̾� Ž�� ����
	public float EnemyRoveRadius; //�� ��Ȳ �� ���� ��ġ �Ҵ� ����
}
