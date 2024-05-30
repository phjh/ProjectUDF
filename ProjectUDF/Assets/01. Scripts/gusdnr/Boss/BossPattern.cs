using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : ScriptableObject
{
	public bool IsActive= false;
	public bool IsThisPassive = false;
	public float PassiveCool = 0f;

	public BossMain.BossState[] NextState;

	protected BossMain bossMain;
	public virtual void Initialize(BossMain bossMain)
	{
		this.bossMain = bossMain;
	}

	public virtual void EnterPattern() { }
	public virtual void ActivePattern() { }
	public virtual void ExitPattern() { }
}
