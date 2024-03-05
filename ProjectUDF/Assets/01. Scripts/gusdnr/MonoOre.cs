using UnityEngine;

public class MonoOre : MonoBehaviour
{
	public Stats IncreaseStat;
	public float IncreaseValue;

	public void GetOre()
	{
		OreInventory.Instance.IncreaseOre(IncreaseStat, IncreaseValue);

	}
}
