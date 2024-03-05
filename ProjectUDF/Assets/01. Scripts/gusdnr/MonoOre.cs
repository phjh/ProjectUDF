using UnityEngine;

public class MonoOre : MonoBehaviour
{
	public PlayerStat stat;

	private void Start()
	{
		stat = GetComponent<PlayerStat>();
	}

	public Stats IncreaseStat;
	public float IncreaseValue;

	public void GetOre()
	{
		stat.EditStat(Stats.Strength, IncreaseValue);
	}
}
