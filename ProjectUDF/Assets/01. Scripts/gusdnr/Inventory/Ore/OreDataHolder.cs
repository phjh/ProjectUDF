using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreDataHolder : MonoBehaviour
{
    private Image image;


	private void Start()
	{
		image = GetComponent<Image>();
	}

	public void OnBoardData(OreSO OreData)
	{
		//������ ���� ��� ����� �̺�Ʈ
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}
}
