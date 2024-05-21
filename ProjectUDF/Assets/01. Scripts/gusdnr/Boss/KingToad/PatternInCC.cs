using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_InCC", menuName = "SO/Boss/Pattern/Pattern_InCC")]
public class PatternInCC : BossPattern
{
	public override void ExitPattern()
	{
		bossMain.IsHaveCC = false;
	}
}
