using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
	public ItemDataSO[] InventoryArray;

	private void OnEnable()
	{
		if (InventoryArray == null) Debug.Log("Item Array is null");
		InventoryArray = Resources.LoadAll<ItemDataSO>("ItemDatas");
	}
}
