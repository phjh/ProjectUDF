using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
	[SerializeField] private GameObject OrePrefab;
	[SerializeField] private RectTransform OreParent;

	public List<GameObject> OrePrefabList;

	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;

	private void AddOreIcon(OreSO data)
	{
		GameObject newOre = Instantiate(OrePrefab);
		newOre.transform.parent = OreParent;
		newOre.transform.localPosition = Vector3.zero;
		OrePrefabList.Add(newOre);

		OreDataHolder soHoledr = newOre.GetComponent<OreDataHolder>();
		soHoledr.OnBoardData(data);
	}

	private void SetOreList()
	{
		OrePrefabList.Clear();
		for (int i = 0; i < 4; i++)
		{
			int createPrefabCount = OreInventory.Instance.OreList[i];
			if (createPrefabCount == 0)
			{
				for (int j = 0; j < createPrefabCount; j++)
				{
					AddOreIcon(OreDatas[i]);
				}
			}
		}
		for (int i = 0; i < 4; i++)
		{
			int createPrefabCount = OreInventory.Instance.GemList[i];
			if (createPrefabCount != 0)
			{
				for (int j = 0; j < createPrefabCount; j++)
				{
					AddOreIcon(GemDatas[i]);
				}
			}
		}
	}

	public void Show()
	{

	}

	public void Close()
	{

	}
}
