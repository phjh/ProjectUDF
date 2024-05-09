using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreInfo : UIMono
{
	[Serializable]
	public struct SlotPair
	{
		public Button SlotButton;
		public OreSlot SlotObject;
	}

	[Header("OreInfo Value")]
	public TMP_Text OreName;
	public TMP_Text OreDesc;
	public List<SlotPair> SlotList = new List<SlotPair>();

	private bool IsShowed = false;

    public OreDataHolder SelectIcon { get; set; } = null;

	public override void ShowUI()
	{
		Debug.Log("Active Info");
		if(IsShowed == false)
		{
			gameObject.SetActive(true);
			IsShowed = true;
		}
		SetOreInfo();
		SetClickEvent();
	}

	public override void CloseUI()
	{
		OreName.text = "";
		OreDesc.text = "";
		SelectIcon = null;
		gameObject.SetActive(false);
		IsShowed = false;
	}

	public void SetUpHolder(OreDataHolder selected) => SelectIcon = selected;

	private void SetClickEvent()
	{
		if (SelectIcon == null) return;
		SlotList[0].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(-1));
		SlotList[1].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(0));
		SlotList[2].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(1));
	}

	private void SetOreInfo()
	{
		OreSO data = SelectIcon.HoldingData;
		OreName.text = $"{data.OreName}";
		string desc = "";
		switch (data.stat)
		{
			case Stats.Strength: desc = "힘"; break;
			case Stats.Lucky: desc = "행운"; break;
			case Stats.MoveSpeed: desc = "이동 속도"; break;
			case Stats.AttackSpeed: desc = "공격 속도"; break;
			default: break;
		}
		desc += $"\n[{data.value} 증가]";
		if (data.valuePersent != 0) desc += $"\n[{data.valuePersent}% 증가]";
		OreDesc.text = desc;
	}

}
