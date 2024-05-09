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
		//현재 광석의 데이터를 설명 보드 측으로 보내주거나, 설명하는 UI 등장
	}

	public void PrintOreDesc()
	{
		UIManager.Instance.OreInfo.SetActive(true);
		UIManager.Instance.OreName.text = $"{HoldingData.OreName}";
		string desc = "";
		switch(HoldingData.stat)
		{
			case Stats.Strength: desc = "힘"; break;
			case Stats.Lucky: desc = "행운"; break;
			case Stats.MoveSpeed: desc = "이동 속도"; break;
			case Stats.AttackSpeed: desc = "공격 속도"; break;
			default: break;
		}
		desc += $"\n[{HoldingData.value} 증가]";
		if(HoldingData.valuePersent != 0) desc += $"\n[{HoldingData.valuePersent}% 증가]";
		UIManager.Instance.OreDesc.text = desc;
	}

	public void PrintNone()
	{
		UIManager.Instance.OreName.text = $" ";
		UIManager.Instance.OreDesc.text = $" ";
	}

	public void EquipOreData(int SubIndex = -1)
	{
		if(SubIndex == -1) OreInventory.Instance.EquipMain(HoldingData.stat);
		if(SubIndex != -1) OreInventory.Instance.EquipSub(HoldingData.stat, SubIndex);

		Destroy(gameObject);
	}
}
