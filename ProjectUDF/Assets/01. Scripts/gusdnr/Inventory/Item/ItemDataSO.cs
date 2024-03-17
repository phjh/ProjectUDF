using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStatusData
{
	public Stats UsingStat;
	public float StatValue = 0;
	public bool isPersent = false;
}

[System.Serializable]
public struct ItemCollect
{
	public ItemDataSO ItemData;
	public bool IsThisCollect;
}

[CreateAssetMenu(fileName = "Empty Item Data", menuName = "SO/Player/ItemSO")]
public class ItemDataSO : ScriptableObject
{
	[Header("������ ����")]
	public string ItemID = "999";
	public string ItemName;
	public string ItemDescription;

	[Header("������ �� ���� �ɷ�ġ")]
	public List<ItemStatusData> StatusDatas = new List<ItemStatusData>();
}
