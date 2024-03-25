using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoSingleton<InventoryUIManager>
{
	[Header("Need To Run")]
	[SerializeField] private GameObject OrePrefab;
	[SerializeField] private RectTransform OreParent;
	[SerializeField] private GameObject Pocket;

	[HideInInspector] public List<GameObject> IconList;
	[Header("Data List")]
	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;

	private List<int> InOreList; //실제 리스트 할당
	private List<int> InGemList; //실제 리스트 할당
	private bool isOpenUI = false;

	private void Awake()
	{
		SetOreList();
		isOpenUI = false;
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
		if (isOpenUI == false)
		{
			UIManager.Instance.SetScreenFilter(true);
			Pocket.SetActive(true);
			SetOreList();
			isOpenUI = true;
		}
	}

	public void Close()
	{
		if (isOpenUI == true)
		{
			for (int i = 0; i < IconList.Count; i++) Destroy(Instance.IconList[i]);
			IconList.Clear();
			Pocket.SetActive(false);
			UIManager.Instance.SetScreenFilter(false);
			isOpenUI = false;
		}
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
		OreDataHolder soHoledr = newOre.GetComponent<OreDataHolder>();
		soHoledr.SettingOreData(data);

		newOre.transform.SetParent(OreParent);
		newOre.name = newOre.name.Replace("(Clone)", $"[{soHoledr.HoldingData.name}]");
		newOre.transform.localPosition = new Vector3(Random.Range(-200, 200), 0, 0);
		IconList.Add(newOre);

	}

	#endregion

}
