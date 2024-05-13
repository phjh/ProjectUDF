using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreDataHolder : MonoBehaviour
{
	private Image HolderImage;
	public OreSO HoldingData;

	private void Awake()
	{
		HolderImage = GetComponent<Image>();
	}

	public void SettingOreData(OreSO OreData)
	{
		HoldingData = OreData;
		HolderImage.sprite = OreData.OreSprite;
	}

	public void ShowOreInfo()
	{
		UIManager.Instance._OreInfo.SetUpHolder(this);
		UIManager.Instance._OreInfo.ShowUI();
		UIManager.Instance.OreName.text = $"{HoldingData.OreName}";
		string desc = "";
		switch (HoldingData.stat)
		{
			case Stats.Strength: desc = "��"; break;
			case Stats.Lucky: desc = "���"; break;
			case Stats.MoveSpeed: desc = "�̵� �ӵ�"; break;
			case Stats.AttackSpeed: desc = "���� �ӵ�"; break;
			default: break;
		}
		desc += $"\n[{HoldingData.value} ����]";
		if (HoldingData.valuePersent != 0) desc += $"\n[{HoldingData.valuePersent}% ����]";
		UIManager.Instance.OreDesc.text = desc;
	}

	public void EquipOreData(int SubIndex)
	{
		if (SubIndex == -1)
		{
			OreInventory.Instance.EquipMain(HoldingData.stat);
			UIManager.Instance._OreInfo.CloseUI();
			Destroy(gameObject);
		}
		else if (SubIndex > -1 && SubIndex <= OreInventory.Instance.SubOreType.Count)
		{
			if (OreInventory.Instance.MainOreType != Stats.None)
			{
				OreInventory.Instance.EquipSub(HoldingData.stat, SubIndex);
				UIManager.Instance._OreInfo.CloseUI();
				Destroy(gameObject);
			}
		}
		Debug.Log($"Main {OreInventory.Instance.MainOreType} : Sub {OreInventory.Instance.SubOreType[0]} {OreInventory.Instance.SubOreType[1]}");

	}
}
