using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoSingleton<InventoryUIManager>
{
	[SerializeField] private GameObject OrePrefab;
	[SerializeField] private RectTransform OreParent;
	[SerializeField] private GameObject Background;

	public List<GameObject> OrePrefabList;

	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;

	private void Start()
	{
		SetOreList();
		Close();
	}

	private void AddOreIcon(OreSO data)
	{
		GameObject newOre = Instantiate(OrePrefab);
		newOre.transform.SetParent(OreParent);
		newOre.transform.localPosition = Vector3.zero;
		OrePrefabList.Add(newOre);

		OreDataHolder soHoledr = newOre.GetComponent<OreDataHolder>();
		soHoledr.SettingOreData(data);
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
		Background.SetActive(true);
		SetOreList();
	}

	public void Close()
	{
		Background.SetActive(false);
		OrePrefabList?.ForEach(prefab => 
		{
			OrePrefabList.Remove(prefab);
			Destroy(prefab);
		});
	}
}
