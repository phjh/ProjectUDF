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
		//���� ������ �κ��丮 UI�� �߰����ִ� �κ� �߰�
	}

	public void DropItem(int count) // ���� ������Ʈ�� �����ϴ� �������� �� ��
	{
		for (int i = 0; i < count; i++)
		{
			ItemDataSO dropItem = DropItemList[Random.Range(0, DropItemList.Count)];
			//���� ��� �������� ������ ��� ������Ʈ �������� ����
			playerInventory.AddItemInInventory(dropItem);
		}
	}
}
