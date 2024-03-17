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
	[Header("아이템 정보")]
	public string ItemID = "999";
	public string ItemName;
	public string ItemDescription;

	[Header("아이템 실 적용 능력치")]
	public List<ItemStatusData> StatusDatas = new List<ItemStatusData>();
}
