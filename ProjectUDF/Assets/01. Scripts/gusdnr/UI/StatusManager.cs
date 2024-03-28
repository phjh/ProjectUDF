using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
	private PlayerStat playerStat;

	[Header("HP UI Value")]
    [SerializeField] private GameObject HeartPrefab;
    [SerializeField] private float xSpacing;
    [SerializeField] private float ySpacing;
	private GameObject[] objects;
	private List<HeartState> hearts = new List<HeartState>();


	private void OnEnable()
	{
		PlayerStat.HpChanged += DrawHearts;
		PlayerStat.OnDeadPlayer += HideStatus;
	}

	private void OnDisable()
	{
		PlayerStat.HpChanged -= DrawHearts;
	}

	private void Awake()
	{
		SettingHearts();
		objects = GetComponentsInChildren<GameObject>();
	}

	#region 체력 부분 스크립트
	public void SettingHearts()
	{
		playerStat = GameManager.Instance.player._playerStat;

		int heartToMake = playerStat.MaxHP;
		for (int i = 0; i < heartToMake; i++)
		{
			CreateEmptyHearts();
		}
		DrawHearts();
	}

	public void DrawHearts()
	{
		for(int i = 0; i < hearts.Count; i++)
		{
			int heartStatusRemainder = (int)Mathf.Clamp(playerStat.CurHP - i, 0 , 1);
			hearts[i].SetHeartImage((UIHeartState)heartStatusRemainder);
		}
	}

	public void CreateEmptyHearts()
	{
		GameObject newHeart = Instantiate(HeartPrefab);
		newHeart.transform.SetParent(gameObject.transform);
		newHeart.name = newHeart.name.Replace("(Clone)", "");
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

	#endregion

	#region Methods
	public void ShowStatus()
	{
		foreach (GameObject obj in objects)
		{
			obj.SetActive(true);
		}
	}
	public void HideStatus()
	{
		foreach(GameObject obj in objects)
		{
			obj.SetActive(false);
		}
	}
	#endregion
}
