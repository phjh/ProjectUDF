using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreDataHolder : MonoBehaviour
{
    private Image image;
	public OreSO HoldingData;

	private void Start()
	{
		image = GetComponent<Image>();
	}

	public void SettingOreData(OreSO OreData)
	{
		HoldingData = OreData;
		//������ ���� ��� ����� �̺�Ʈ
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}
}
