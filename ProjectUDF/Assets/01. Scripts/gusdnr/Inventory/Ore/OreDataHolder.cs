using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreDataHolder : MonoBehaviour
{
    private Image HolderImage;
	public OreSO HoldingData;

	public void SettingOreData(OreSO OreData)
	{
		Debug.Log("Method");
		HolderImage = GetComponent<Image>();
		HoldingData = OreData;
		HolderImage.sprite = OreData.OreSprite;
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}
}
