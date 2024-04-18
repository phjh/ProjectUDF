using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Area Atttack", menuName = "SO/State/Attack/Area")]
public class AreaAttackState : EnemyState
{
	public override EnemyState Clone()
	{
		AreaAttackState clone = CloneBase() as AreaAttackState;
		//Setting public value setting

		return clone;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
	}
}
