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
		if (IsShowed == false)
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
		if (SelectIcon == null) return; //���� ó��

		if (SelectIcon.HoldingData.valuePersent == 0)
		{
			SlotList.ForEach(pair =>
			{
				pair.SlotButton.gameObject.SetActive(true);
				pair.SlotButton.interactable = true;
			});
		}
		else if (SelectIcon.HoldingData.valuePersent != 0)
		{
			SlotList.ForEach(pair =>
			{
				pair.SlotButton.gameObject.SetActive(false);
			});
		}

		if(OreInventory.Instance.MainOreType == Stats.None)
		{
			SlotList[1].SlotButton.interactable = false;
			SlotList[2].SlotButton.interactable = false;
		}

		SlotList[0].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(-1));
		SlotList[0].SlotButton.onClick.AddListener(() => SlotList[0].SlotObject?.ShowUI());

		SlotList[1].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(0));
		SlotList[1].SlotButton.onClick.AddListener(() => SlotList[1].SlotObject?.ShowUI());

		SlotList[2].SlotButton.onClick.AddListener(() => SelectIcon?.EquipOreData(1));
		SlotList[2].SlotButton.onClick.AddListener(() => SlotList[2].SlotObject?.ShowUI());
	}

	private void SetOreInfo()
	{
		OreSO data = SelectIcon.HoldingData;
		OreName.text = $"{data.OreName}";
		string desc = "";
		switch (data.stat)
		{
			case Stats.Strength: desc = "��"; break;
			case Stats.Lucky: desc = "���"; break;
			case Stats.MoveSpeed: desc = "�̵� �ӵ�"; break;
			case Stats.AttackSpeed: desc = "���� �ӵ�"; break;
			default: break;
		}
		desc += $"\n[{data.value} ����]";
		if (data.valuePersent != 0) desc += $"\n[{data.valuePersent}% ����]";
		OreDesc.text = desc;
	}

}