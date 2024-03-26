using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackPatternBase : MonoBehaviour
{
	protected EnemyMain enemy;

	public EnemyAttackPatternBase(EnemyMain enemy)
	{
		this.enemy = enemy;
	}

	public abstract void ExecuteAttack();

	public abstract bool IsAttackFinished();
}
