using UnityEngine;

public class BossPassive : ScriptableObject
{
	[HideInInspector] protected BossMain bossMain;
	public virtual void Initialize(BossMain bossMain)
	{
		this.bossMain = bossMain;
	}

    public float ActiveTickTime;
	public virtual void PassiveActive() { }
}
