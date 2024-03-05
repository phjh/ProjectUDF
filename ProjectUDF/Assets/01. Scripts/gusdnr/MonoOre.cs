using UnityEngine;

public class MonoOre : MonoBehaviour
{
	public Stats IncreaseStat;
	[Range(0, 1)]public float IncreaseValue;

	public void GetOre()
	{
		OreInventory.Instance.IncreaseOre(IncreaseStat, IncreaseValue);
		Debug.Log(gameObject.name);
	}
}
