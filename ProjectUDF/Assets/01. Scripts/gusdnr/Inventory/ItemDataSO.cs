using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStatusPair
{
	public Stats UsingStat;
	public float StatValue;
	public bool isPersent;
}

[CreateAssetMenu(fileName = "Empty Item Data", menuName = "SO/Player/ItemSO")]
public class ItemDataSO : ScriptableObject
{
	[Header("������ ����")]
	public string ItemID = "999";
	public string ItemName;
	public string ItemDescription;

	[Header("������ �� ���� �ɷ�ġ")]
	public List<ItemStatusPair> StatusDatas = new List<ItemStatusPair>();
}
