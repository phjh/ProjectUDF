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
		status = GameManager.Instance.player._playerStat;
	}

	public void AddItemInInventory(ItemDataSO data)
    {
		ItemManager.Instance.CollectItem(data);
        CollectItems.Add(data);
		ReadingData(data);
    }

	private void ReadingData(ItemDataSO data)
	{
		List<ItemStatusData> statusData = data.StatusDatas;
		ItemStatusData editData;
		for (int i = 0; i < statusData.Count; i++)
		{
			editData = statusData[i];
			status.EditModifierStat(editData.UsingStat, editData.StatValue, editData.isPersent);
		}
	}
}
