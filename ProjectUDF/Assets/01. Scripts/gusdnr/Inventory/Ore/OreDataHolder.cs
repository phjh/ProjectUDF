using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDataHolder : MonoBehaviour
{
    public OreSO HoldData;

    private SpriteRenderer spriteRenderer;


	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnBoardData()
	{
		//������ ���� ��� ����� �̺�Ʈ
		//���� ������ �����͸� ���� ���� ������ �����ְų�, �����ϴ� UI ����
	}
}
