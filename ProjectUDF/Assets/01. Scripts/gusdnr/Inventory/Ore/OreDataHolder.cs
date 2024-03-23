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
		//광석을 누를 경우 실행될 이벤트
		//현재 광석의 데이터를 설명 보드 측으로 보내주거나, 설명하는 UI 등장
	}
}
