using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<ItemDataSO> CollectItems;
	private PlayerStat status;

	private void OnEnable()
	{
		CollectItems = new List<ItemDataSO>(); //���ο� ����Ʈ�� ����� �Ҵ��Ѵ�.
	}

	public void AddItemInInventory(ItemDataSO data)
    {
        CollectItems.Add(data);
    }

	private void ReadingData(ItemDataSO data)
	{
		List<ItemStatusData> itemData = data.StatusDatas;
		ItemStatusData editData;
		for (int i = 0; i < itemData.Count; i++)
		{
			editData = itemData[i];
			status.EditModifierStat(editData.UsingStat, editData.StatValue, editData.isPersent);
		}
	}
}
