using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : ScriptableObject
{
	[HideInInspector] public bool IsActive= false;
	public float IfPassiveCool = 0f;

	public BossMain.BossState[] NextState;

	protected BossMain bossMain;
	public virtual void Initialize(BossMain bossMain)
	{
		this.bossMain = bossMain;
	}

	public virtual void EnterPattern()
	{
		IsActive = true;
	}
	public virtual void ActivePattern()
	{
		if (IsActive == false)
		{
			ExitPattern();
		}
	}
	public virtual void ExitPattern()
	{
		IsActive = false;
	}
}
