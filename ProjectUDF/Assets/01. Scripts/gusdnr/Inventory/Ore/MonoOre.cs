using UnityEngine;

public class MonoOre : MonoBehaviour
{
	public Stats IncreaseStat;
	public int IncreaseValue = 3;

	public void GetOre() => OreInventory.Instance.AddOre(IncreaseStat, IncreaseValue);
}
