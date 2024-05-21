using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_Idle", menuName = "SO/Boss/Pattern")]
public class PatternInCC : BossPattern
{

	public override void EnterPattern()
	{
	}

	public override void ActivePattern()
	{
		
	}

	public override void ExitPattern()
	{
		bossMain.IsHaveCC = false;
	}
}
