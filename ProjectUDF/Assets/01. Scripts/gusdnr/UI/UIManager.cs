using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
	public GameObject miningCanvas;
	public List<GameObject> Cards;

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
}
