using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void DoingAttack(EnemyBase eb);
}

public abstract class AtkPatternMono : MonoBehaviour, IAttack
{
	public virtual void DoingAttack(EnemyBase eb)
	{
		Debug.Log("Attack Active");
	}
}