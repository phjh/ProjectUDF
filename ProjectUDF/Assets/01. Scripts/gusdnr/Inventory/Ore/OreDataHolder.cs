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
		InventoryUIManager.Instance.OnClickedIcon += PrintOreDesc;
	}

	private void OnDestroy()
	{
		InventoryUIManager.Instance.OnClickedIcon -= PrintOreDesc;
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
		InventoryUIManager.Instance.
	}
}
