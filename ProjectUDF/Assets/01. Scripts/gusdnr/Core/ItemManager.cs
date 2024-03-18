using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
	public ItemInventory playerInventory;

	public ItemDataSO[] InventoryArray;
	private List<ItemDataSO> DropItemList = new List<ItemDataSO>();

	private void OnEnable()
	{
		if (InventoryArray == null) Debug.Log("Item Array is null");
		InventoryArray = Resources.LoadAll<ItemDataSO>("ItemDatas");

		for(int i = 0; i < InventoryArray.Length; i++)
		{
			if (InventoryArray[i].isHidden == false)
			{
				DropItemList.Add(InventoryArray[i]);
			}
		}
	}

	public void CollectItem(ItemDataSO data)
	{
		InventoryArray[data.ItemID].isCollect = true;
		DropItemList.RemoveAt(DropItemList.IndexOf(data));
		//추후 아이템 인벤토리 UI에 추가해주는 부분 추가
	}

	public void DropItem(int count) // 추후 오브젝트를 생성하는 방향으로 갈 것
	{
		for (int i = 0; i < count; i++)
		{
			ItemDataSO dropItem = DropItemList[Random.Range(0, DropItemList.Count)];
			//추후 드롭 아이템의 정보가 담긴 오브젝트 생성으로 변경
			playerInventory.AddItemInInventory(dropItem);
		}
	}
}
