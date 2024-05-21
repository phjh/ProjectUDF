using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_KingToadIdle", menuName = "SO/Boss/KingToad/Pattern_KingToadIdle")]
public class PatternKingToadIdle : BossPattern
{
	public override void EnterPattern()
	{
		if(bossMain.IsCooldown == false && bossMain.IsAttack == false)
		{
			bossMain.SetState(BossMain.BossState.Attack);
		}
		else if (bossMain.IsCooldown == true)
		{
			bossMain.SetState(BossMain.BossState.Cooldown);
		}
		else if(bossMain.IsHaveCC == true)
		{
			bossMain.SetState(BossMain.BossState.InCC);
		}
	}
}
