using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStates
{
	Lobby = 0,
	Play = 1,
	NonPauseUIOn = 2,
	PauseUIOn = 3,
	End = 4,
}

public enum GameResults
{
	Play = 0,
	TimeOut = 1,
	DiePlayer = 2,
}

public class GameManager : MonoSingleton<GameManager>
{
	#region Pooling
	[Header("Pooling")]
	[SerializeField]
	private PoolingListSO poollistSO;
	[SerializeField]
	private Transform _poolingTrm;
	#endregion

	#region Player Info
	[Header("Player")]
	public Player player;

	[Header("Player Stat")]
	public float Strength;
	public float Lucky;
	public float AttackSpeed;
	public float MoveSpeed;
	#endregion

	[Header("Item Manage")]
	public ItemDataSO[] InventoryArray;
	private List<ItemDataSO> DropItemList = new List<ItemDataSO>();
	private ItemInventory playerInventory;

	#region In Game Flow
	public GameStates gameState;
	public GameResults gameResult;
	#endregion

	private void OnEnable()
	{
		if (InventoryArray == null) Debug.Log("Item Array is null");
		InventoryArray = Resources.LoadAll<ItemDataSO>("ItemDatas");

		for (int i = 0; i < InventoryArray.Length; i++)
		{
			if (InventoryArray[i].isHidden == false)
			{
				DropItemList.Add(InventoryArray[i]);
			}
		}
	}

	private void Awake()
	{
		PoolManager.Instance = new PoolManager(_poolingTrm);
		foreach (var obj in poollistSO.PoolObjtectList)
		{
			PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
		}
		foreach (var obj in poollistSO.PoolEffectLists)
		{
			PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
		}
		if (player == null) player = FindObjectOfType<Player>().GetComponent<Player>();
		if (playerInventory == null) playerInventory = player.GetComponent<ItemInventory>();
	}

	#region Methods

	public void CollectItem(ItemDataSO data)
	{
		InventoryArray[data.ItemID].isCollect = true;
		DropItemList.RemoveAt(DropItemList.IndexOf(data));
		//추후 아이템 인벤토리 UI에 추가해주는 부분 추가
	}

	public void DropItem(int count) // 추후 오브젝트를 생성하는 방향으로 갈 것
	{
		for (int i = 0; i < count; i++)
		{
			ItemDataSO dropItem = DropItemList[UnityEngine.Random.Range(0, DropItemList.Count)];
			//추후 드롭 아이템의 정보가 담긴 오브젝트 생성으로 변경
			playerInventory.AddItemInInventory(dropItem);
		}
	}

	public void ReloadStats()
	{
		Lucky = player._playerStat.Lucky.GetValue();
		Strength = player._playerStat.Strength.GetValue();
		MoveSpeed = player._playerStat.MoveSpeed.GetValue();
		AttackSpeed = player._playerStat.AttackSpeed.GetValue();
	}

	public void UpdateState(GameStates SetState)
	{
		gameState = SetState;
		StateChecker();
	}

	private void StateChecker()
	{
		switch (gameState)
		{
			//각 게임 스테이트별 실행할 행동 설정
		}
	}

	public void UpdateResult(GameResults SetResult)
	{
		gameResult = SetResult;
		if(gameResult != GameResults.Play) UpdateState(GameStates.End);
	}

	#endregion

}