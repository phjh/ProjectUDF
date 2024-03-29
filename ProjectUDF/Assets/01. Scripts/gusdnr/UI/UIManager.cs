using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	[Header("UI Objects")]
	public GameObject ScreenFilter;
	public List<OreCard> Cards;

	private int failCount = 0;
	
	private bool isActivePopUp;
	public bool IsActivePopUp
	{
		get { return isActivePopUp; }
		set { isActivePopUp = value; }
	}

	public static event EventHandler OnResearchEnd;

	private void Awake()
	{
		SetScreenFilter(false);
	}

	#region Mining UI

	public void ShowCards()
	{
		failCount = 0;
		SetScreenFilter(true);
		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].GetComponent<OreCard>().Show();
		}
	}

	public void HideCards()
	{
		SetScreenFilter(false);
	}

	public void CountFail()
	{
		failCount += 1;
		Debug.Log(failCount);
		if(failCount == 3)
		{
			for (int i = 0; i < Cards.Count; i++)
			{
				Cards[i].GetComponent<OreCard>().HideDefault();
			}
		}
	}

	#endregion

	#region Public Method

	public void SetScreenFilter(bool isActive)
	{
		ScreenFilter?.SetActive(isActive);
	}

	#endregion

}
