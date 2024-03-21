using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	[Header("UI Objects")]
	public GameObject ScreenFilter;
	public List<GameObject> Cards;

	private int failCount = 0;

	public static event EventHandler OnResearchEnd;

	private void Start()
	{
		ShowCards();
	}

	public void ShowCards()
	{
		failCount = 0;
		ScreenFilter?.SetActive(true);
		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].GetComponent<PanelHandler>().Show();
		}
	}

	public void HideCards()
	{
		ScreenFilter?.SetActive(false);
	}

	public void CountFail()
	{
		failCount += 1;
		Debug.Log(failCount);
		if(failCount == 3)
		{
			for (int i = 0; i < Cards.Count; i++)
			{
				Cards[i].GetComponent<PanelHandler>().HideDefault();
			}
		}
	}
	
}
