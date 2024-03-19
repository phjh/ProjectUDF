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
		//광석을 누를 경우 실행될 이벤트
		//현재 광석의 데이터를 설명 보드 측으로 보내주거나, 설명하는 UI 등장
	}
}
