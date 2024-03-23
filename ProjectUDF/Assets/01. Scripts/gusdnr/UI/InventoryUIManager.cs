using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoSingleton<InventoryUIManager>
{
	[SerializeField] private GameObject OrePrefab;
	[SerializeField] private RectTransform OreParent;
	[SerializeField] private GameObject Background;

	public List<GameObject> IconList;

	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;

	private List<int> InOreList;
	private List<int> InGemList;


	private void Start()
	{
		SetOreList();
	}

	private void SetOreList()
	{
		IconList.Clear();
		InOreList = OreInventory.Instance.OreList;
		CalculateInventory(InOreList, OreDatas);

		InGemList = OreInventory.Instance.GemList;
		CalculateInventory(InGemList, GemDatas);
	}

	public void Show()
	{
		Background.SetActive(true);
		SetOreList();
	}

	public void Close()
	{
		Background.SetActive(false);
		for(int i = 0; i < IconList.Count; i++) Destroy(Instance.IconList[i]);
		IconList.Clear();
	}

	#region Methods

	private void CalculateInventory(List<int> baseList, List<OreSO> dataList) //Calculate To In OreInventoryList
	{
		for (int i = 0; i < 4; i++)
		{
			int createPrefabCount = baseList[i];
			if (createPrefabCount != 0)
			{
				for (int j = 0; j < createPrefabCount; j++)
				{
					AddOreIcon(dataList[i]);
				}
			}
		}
	}

	private void AddOreIcon(OreSO data) //Make Ore Icon Image
	{
		GameObject newOre = Instantiate(OrePrefab);
		Debug.Log(newOre.name);
		newOre.transform.SetParent(OreParent);
		newOre.transform.position = Vector3.zero;
		IconList.Add(newOre);

		OreDataHolder soHoledr = newOre.GetComponent<OreDataHolder>();
		soHoledr.SettingOreData(data);
	}

	#endregion

}
