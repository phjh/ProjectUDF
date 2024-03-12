using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void DoingAttack(EnemyBase eb);
}

public class EnemyAttack : MonoBehaviour, IAttack
{
	public void DoingAttack(EnemyBase eb)
	{
		Debug.Log("Active Attack");
	}
}