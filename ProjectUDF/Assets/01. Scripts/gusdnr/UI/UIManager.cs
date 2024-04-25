using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	#region UI Objects
	[Header("Default")]
	public GameObject ScreenFilter;

	[Header("Ore Pocket")]
	public GameObject OrePrefab;
	public RectTransform PocketUIParent;

	[Header("Ore Desc")]
	public TMP_Text OreName;
	public TMP_Text OreDesc;
	#endregion

	#region Ore Inventory
	[Header("Data List")]
	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;
	[HideInInspector] public List<GameObject> IconList;
	[HideInInspector] public List<OreCard> Cards;

	private List<int> InOreList; //실제 리스트 할당
	private List<int> InGemList; //실제 리스트 할당
	private bool IsOnInventoryUI = false;
	#endregion

	#region Mining
	private int failCount = 0;
	public bool IsActivePopUp {	get; set; } = false;
	public static event EventHandler OnResearchEnd;
	#endregion

	private void Awake()
	{
		SetOreList();
		IsOnInventoryUI = false;
		PocketUIParent.gameObject.SetActive(IsOnInventoryUI);
		SetScreenFilter(IsOnInventoryUI);
	}
    private void Start()
    {
        //MapSystem.Instance.RoomClearEvent += ShowMining;
    }

	#region Mining UI
	public void CountFail()
	{
		failCount += 1;
		Debug.Log(failCount);
		if(failCount == 3)
		{
			for (int i = 0; i < Cards.Count; i++)
			{
				Cards[i].GetComponent<OreCard>().CloseDefault();
			}
		}
	}
	#endregion

	#region Ore Pocket UI

	private void SetOreList() //Setting Ores
	{
		IconList.Clear();
		InOreList = OreInventory.Instance.OreList;
		CalculateInventory(InOreList, OreDatas);

		InGemList = OreInventory.Instance.GemList;
		CalculateInventory(InGemList, GemDatas);
	}

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

		newOre.transform.SetParent(PocketUIParent);
		newOre.name = newOre.name.Replace("(Clone)", $"[{soHoledr.HoldingData.name}]");
		newOre.transform.localPosition = new Vector3(UnityEngine.Random.Range(-200, 200), 0, 0);
		IconList.Add(newOre);

	}

	#endregion

	#region Manage UI
	public void ShowPocket()
	{
		if (IsOnInventoryUI == false && IsActivePopUp == false)
		{
			SetScreenFilter(true);
			PocketUIParent.gameObject.SetActive(true);
			SetOreList();
			IsOnInventoryUI = true;
		}
		GameManager.Instance.UpdateState(GameStates.PauseUIOn);
	}

	public void ClosePocket()
	{
		if (IsOnInventoryUI == true)
		{
			for (int i = 0; i < IconList.Count; i++) Destroy(Instance.IconList[i]);
			IconList.Clear();
			PocketUIParent.gameObject.SetActive(false);
			SetScreenFilter(false);
			IsOnInventoryUI = false;
		}
		GameManager.Instance.UpdateState(GameStates.Playing);
	}

	public void ShowMining()
	{
		if(IsOnInventoryUI == true) CloseMining();
		IsActivePopUp = true;
		failCount = 0;
		SetScreenFilter(IsActivePopUp);
		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].GetComponent<OreCard>().ShowUI();
		}
		GameManager.Instance.UpdateState(GameStates.NonPauseUIOn);
	}

	public void CloseMining()
	{
		IsActivePopUp = false;
		SetScreenFilter(IsActivePopUp);
		GameManager.Instance.UpdateState(GameStates.Playing);
	}
	#endregion

	#region Methods

	public void SetScreenFilter(bool isActive)
	{
		ScreenFilter?.SetActive(isActive);
	}

	#endregion
}
