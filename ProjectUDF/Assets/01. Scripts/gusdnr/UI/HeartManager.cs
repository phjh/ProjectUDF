using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private GameObject HeartPrefab;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private float xSpacing;
    [SerializeField] private float ySpacing;

    private List<HeartState> hearts = new List<HeartState>();

	private void OnEnable()
	{
		PlayerStat.HpChanged += DrawHearts;
	}

	private void OnDisable()
	{
		PlayerStat.HpChanged -= DrawHearts;
	}

	private void Start()
	{
		DrawHearts();
	}

	public void DrawHearts()
	{
		ClearHearts();

		float maxHealthRemainder = playerStat.MaxHP % 2; //홀수일 경우 하트가 하나 덜 생성되는 것을 방지
		int heartToMake = (int)(playerStat.MaxHP / 2 + maxHealthRemainder);
		
		for(int i = 0; i < heartToMake; i++)
		{
			CreateEmptyHearts();
		}

		for(int i = 0; i < hearts.Count; i++)
		{
			int heartStatusRemainder = (int)Mathf.Clamp(playerStat.CurHP - (i * 2), 0, 2);
			hearts[i].SetHeartImage((UIHeartState)heartStatusRemainder);
		}
	}

	public void CreateEmptyHearts()
	{
		GameObject newHeart = Instantiate(HeartPrefab);
		newHeart.transform.SetParent(gameObject.transform);
		int heartIndex = hearts.Count;

		if(heartIndex == 0)
		{
			newHeart.transform.localPosition = Vector3.zero;
		}
		else
		{
			Vector2 prevHeartPos = hearts[heartIndex - 1].transform.GetComponent<RectTransform>().anchoredPosition;
			Vector2 pos = Vector2.zero;

			if(heartIndex % 2 == 0) pos = new Vector2(prevHeartPos.x + xSpacing, prevHeartPos.y + ySpacing);
			else pos = new Vector2(prevHeartPos.x + xSpacing, prevHeartPos.y - ySpacing);

			newHeart.GetComponent<RectTransform>().anchoredPosition = pos;
		}

		newHeart.transform.localScale = Vector3.one;

		HeartState newHeartState = newHeart.GetComponent<HeartState>();
		newHeartState.SetHeartImage(UIHeartState.Empty);
		hearts.Add(newHeartState);
	}

	public void ClearHearts()
	{
		foreach(Transform t in transform)
		{
			Destroy(t.gameObject);
		}
	}
}
