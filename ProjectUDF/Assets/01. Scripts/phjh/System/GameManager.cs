using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	#region Pool
	[Header("Pool Managing Values")]
    [SerializeField]
    private PoolingListSO poollistSO;
    [SerializeField]
    private Transform _poolingTrm;
	#endregion

	public Player player;

	[Header("Item Manage")]
	public ItemDataSO[] InventoryArray;
	private List<ItemDataSO> DropItemList = new List<ItemDataSO>();
	private ItemInventory playerInventory;

	private void OnEnable()
	{
		if (InventoryArray == null) Debug.Log("Item Array is null");
		InventoryArray = Resources.LoadAll<ItemDataSO>("ItemDatas");

		for (int i = 0; i < InventoryArray.Length; i++)
		{
			if (InventoryArray[i].isHidden == false)
			{
				DropItemList.Add(InventoryArray[i]);
			}
		}
	}

	private void Awake()
    {
        PoolManager.Instance = new PoolManager(_poolingTrm);
        foreach (var obj in poollistSO.list)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }

        if(player == null) player = FindObjectOfType<Player>().GetComponent<Player>();
		if(playerInventory == null) playerInventory = player.GetComponent<ItemInventory>();
	}

	#region Methods

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
			ItemDataSO dropItem = DropItemList[UnityEngine.Random.Range(0, DropItemList.Count)];
			//���� ��� �������� ������ ��� ������Ʈ �������� ����
			playerInventory.AddItemInInventory(dropItem);
		}
	}

	#endregion

}
