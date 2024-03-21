using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	public GameObject miningCanvas;
	public List<GameObject> Cards;

	public static event EventHandler OnResearchEnd;

	private void Start()
	{
		ShowCards();
	}

	public void ShowCards()
	{
		miningCanvas?.SetActive(true);
		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].GetComponent<PanelHandler>().Show();
		}
	}

	public void HideCards()
	{
		miningCanvas?.SetActive(false);
	}

	public int failCount = 0;

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
