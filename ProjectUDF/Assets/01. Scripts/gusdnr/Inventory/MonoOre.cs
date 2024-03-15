using UnityEngine;

public class MonoOre : MonoBehaviour
{
	public Stats IncreaseStat;
	public float IncreaseValue = 3;

	public void GetOre()
	{
		OreInventory.Instance.AddOre(IncreaseStat, IncreaseValue);
		Debug.Log(gameObject.name);
	}
}
