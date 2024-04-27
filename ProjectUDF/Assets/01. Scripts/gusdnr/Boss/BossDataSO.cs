using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Data", menuName = "SO/Boss/Data")]
public class BossDataSO : ScriptableObject
{
	public string BossName;
	public float MaxHP;
}
