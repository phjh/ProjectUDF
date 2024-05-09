using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreInfo : UIMono
{
	[Header("OreInfo Value")]
	public TMP_Text OreName;
	public TMP_Text OreDesc;
    public Button MainEquip;
    public List<Button> SubEquip = new List<Button>(2);

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
		MainEquip.onClick.AddListener(() => SelectIcon?.EquipOreData(-1));
		SubEquip[0].onClick.AddListener(() => SelectIcon?.EquipOreData(0));
		SubEquip[1].onClick.AddListener(() => SelectIcon?.EquipOreData(1));
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
