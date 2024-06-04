using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_KingToadIdle", menuName = "SO/Boss/KingToad/Pattern_KingToadIdle")]
public class PatternKingToadIdle : BossPattern
{
	public override void ActivePattern()
	{
		if (bossMain.IsHaveCC == true)
		{
			ExitPattern();
			bossMain.SetState(NextState[2]);
		}

		if (bossMain.IsCooldown == false && bossMain.IsAttack == false)
		{
			ExitPattern();
			bossMain.SetState(NextState[0]);
		}
		else if (bossMain.IsCooldown == true)
		{
			ExitPattern();
			bossMain.SetState(NextState[1]);
		}
	}
}
