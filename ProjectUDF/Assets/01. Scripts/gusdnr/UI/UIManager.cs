using GameManageDefine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
	#region UI Objects
	[Header("Default")]
	public GameObject ScreenFilter;

	[Header("Ore Pocket")]
	public GameObject OrePrefab;
	public RectTransform PocketUIParent;
	public RectTransform IconContainer;
	public OreInfo _OreInfo;
	public TMP_Text OreName;
	public TMP_Text OreDesc;
	#endregion

	#region Ore Inventory
	[Header("Data List")]
	public List<OreSO> OreDatas;
	public List<OreSO> GemDatas;
	public List<GameObject> IconList;
	[HideInInspector] public List<OreCard> Cards;

	private List<int> InOreList; //실제 리스트 할당
	private List<int> InGemList; //실제 리스트 할당
	private bool IsOnInventoryUI = false;
	#endregion

	#region Mining
	private int failCount = 0;
	public bool IsActivePopUp { get; set; } = false;
	public static event EventHandler OnResearchEnd;
	#endregion

	[Tooltip("맞았을때 뜰 빨간 화면")]
	public Image BloodScreen;

	private void Awake()
	{
		SetOreList();
		IsOnInventoryUI = false;
		PocketUIParent.gameObject.SetActive(IsOnInventoryUI);
		SetScreenFilter(IsOnInventoryUI);
	}
	private void OnEnable()
	{
		MapSystem.Instance.RoomClearEvent += ShowMining;
		OreInventory.Instance.ChangeContents += SetOreList;
	}

	private void OnDisable()
	{
		MapSystem.Instance.RoomClearEvent -= ShowMining;
		//OreInventory.Instance.ChangeContents -= SetOreList;
	}

	#region Mining UI
	public void CountFail()
	{
		failCount += 1;
		Debug.Log(failCount);
		if (failCount == 3)
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
		if (IconList.Count() != 0)
		{
			for (int icon = 0; icon < IconList.Count(); icon++)
			{
				Destroy(Instance.IconList[icon]);
			}
		}
		IconList.Clear();
		IconList = new List<GameObject>();

		InOreList = OreInventory.Instance.OreList;
		CalculateInventory(InOreList, OreDatas);

		InGemList = OreInventory.Instance.GemList;
		CalculateInventory(InGemList, GemDatas);
	}

	public void RemoveIcon(int statNumber)
	{
		bool isRemoveOneIcon = false;

		foreach(GameObject item in IconList)
		{
			if(isRemoveOneIcon) break;

			if (item.TryGetComponent<OreDataHolder>(out var dataHolder))
			{
				if (dataHolder.HoldingData.stat == (Stats)statNumber)
				{
					IconList.Remove(item);
					Destroy(item.gameObject);
					isRemoveOneIcon = true;
				}
			}

			if(isRemoveOneIcon) break;
		};

		SetOreList();
	}

	private void CalculateInventory(List<int> baseList, List<OreSO> dataList) //Calculate To In OreInventoryList
	{
		for (int i = 0; i < (int)Stats.HP; i++)
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

	public void AddOreIcon(OreSO data) //Make Ore Icon Image
	{
		GameObject newOre = Instantiate(OrePrefab);
		OreDataHolder soHolder = newOre.GetComponent<OreDataHolder>();
		soHolder.SettingOreData(data);
		
		newOre.transform.SetParent(IconContainer);
		newOre.name = newOre.name.Replace("(Clone)", $"[{soHolder.HoldingData.name}]");
		newOre.transform.localPosition = new Vector3(UnityEngine.Random.Range(-240, 240), -50, 0);
		IconList.Add(newOre);
	}

	#endregion

	#region Manage UI
	public void ManagePocketUI()
	{
		if (!IsOnInventoryUI && PlayerMain.Instance.IsUIPopuped)
			return;
		if (IsOnInventoryUI == false)
		{
			if (IsActivePopUp == false)
			{
				SetScreenFilter(true);
				PocketUIParent.gameObject.SetActive(true);
				PlayerMain.Instance.IsUIPopuped = true;
				_OreInfo.CloseUI();
				SetOreList();
				IsOnInventoryUI = true;
				GameManager.Instance.UpdateState(GameStates.PauseUIOn);
			}
		}
		else if (IsOnInventoryUI == true)
		{
			for (int i = 0; i < IconList.Count; i++) Destroy(Instance.IconList[i]);
			IconList.Clear();
			PocketUIParent.gameObject.SetActive(false);
			_OreInfo.CloseUI();
			SetScreenFilter(false);
			IsOnInventoryUI = false;
			GameManager.Instance.UpdateState(GameStates.Playing);
			PlayerMain.Instance.IsUIPopuped = false;
		}
	}

	public void ShowMining()
	{
		if (PlayerMain.Instance.IsUIPopuped)
			return;

		if (IsOnInventoryUI == true) CloseMining();
		PlayerMain.Instance.IsUIPopuped = true;
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
		PlayerMain.Instance.IsUIPopuped = false;
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

	public void ShowBloodScreen(float time)
	{
		if (BloodScreen == null)
			Debug.LogError("UIManager BloodScreen Is Null!!!!!");

		Sequence seq = DOTween.Sequence();
		Color nowcolor = BloodScreen.color;
		Color endcolor = nowcolor;
		endcolor.a = 0.8f;
		seq.Append(BloodScreen.DOColor(endcolor, time).SetEase(Ease.InOutSine))
		   .Append(BloodScreen.DOColor(nowcolor, time).SetEase(Ease.OutQuad));
	}

}
