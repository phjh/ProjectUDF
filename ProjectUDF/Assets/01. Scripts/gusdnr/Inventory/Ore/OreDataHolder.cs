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
		Debug.Log("Method");
		HoldingData = OreData;
		HolderImage.sprite = OreData.OreSprite;
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}

	public void PrintOreDesc()
	{
		InventoryUIManager.Instance.OreName.text = $"{HoldingData.OreName}";
		string desc = "";
		switch(HoldingData.stats)
		{
			case Stats.Strength: desc = "��"; break;
			case Stats.Lucky: desc = "���"; break;
			case Stats.MoveSpeed: desc = "�̵� �ӵ�"; break;
			case Stats.AttackSpeed: desc = "���� �ӵ�"; break;
			default: break;
		}
		desc += $"\n[{HoldingData.value} ����]";
		if(HoldingData.valuePersent != 0) desc += $"\n[{HoldingData.valuePersent}% ����]";
		InventoryUIManager.Instance.OreDesc.text = desc;
	}
}
