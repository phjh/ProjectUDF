using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : ScriptableObject
{
	public bool IsActive = false;

	protected BossMain bossMain;
	public virtual void Initialize(BossMain bossMain)
	{
		this.bossMain = bossMain;
	}

	public virtual void EnterPattern() { }
	public virtual void ActivePattern() { }
	public virtual void ExitPattern() { }
}
