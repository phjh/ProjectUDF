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
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}

	public void PrintOreDesc()
	{
		InventoryUIManager.Instance.
	}
}
