using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_KingToadCool", menuName = "SO/Boss/KingToad/Pattern_KingToadCool")]
public class PatternKingToadCool : BossPattern
{
	public override void ExitPattern()
	{
		if(bossMain.IsCooldown)
		{
			bossMain.IsCooldown = false;
		}
	}
}
