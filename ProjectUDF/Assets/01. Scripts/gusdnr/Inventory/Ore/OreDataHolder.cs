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
		UIManager.Instance._OreInfo.GetHolder += SetHolder;
	}

	private void SetHolder(OreDataHolder holder)
	{
		holder = this;
	}

	public void SettingOreData(OreSO OreData)
	{
		Debug.Log("Method");
		HoldingData = OreData;
		HolderImage.sprite = OreData.OreSprite;
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}

	public void ShowOreInfo()
	{
		UIManager.Instance._OreInfo.ShowUI();
		UIManager.Instance.OreName.text = $"{HoldingData.OreName}";
		string desc = "";
		switch(HoldingData.stat)
		{
			case Stats.Strength: desc = "��"; break;
			case Stats.Lucky: desc = "���"; break;
			case Stats.MoveSpeed: desc = "�̵� �ӵ�"; break;
			case Stats.AttackSpeed: desc = "���� �ӵ�"; break;
			default: break;
		}
		desc += $"\n[{HoldingData.value} ����]";
		if(HoldingData.valuePersent != 0) desc += $"\n[{HoldingData.valuePersent}% ����]";
		UIManager.Instance.OreDesc.text = desc;
	}

	public void EquipOreData(int SubIndex = -1)
	{
		if(SubIndex == -1) OreInventory.Instance.EquipMain(HoldingData.stat);
		if(SubIndex != -1) OreInventory.Instance.EquipSub(HoldingData.stat, SubIndex);
		UIManager.Instance._OreInfo.CloseUI();
		Destroy(gameObject);
	}
}
