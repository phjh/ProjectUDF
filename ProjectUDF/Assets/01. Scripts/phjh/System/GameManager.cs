using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStates
{
	Lobby = 0,
	Start = 1,
	Playing = 2,
	NonPauseUIOn = 3,
	PauseUIOn = 4,
	End = 5,
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
	public PoolingListSO poollistSO;
	//[SerializeField]
	//private Transform _poolingTrm;
	#endregion

	#region Player Info
	[Header("Player")]
	public Player player;
	#endregion

	[Header("Item Manage")]
	public ItemDataSO[] InventoryArray;
	private List<ItemDataSO> DropItemList = new List<ItemDataSO>();
	private ItemInventory playerInventory;

	#region In Game Flow
	public GameStates gameState;
	public GameResults gameResult;
	#endregion

	#region Game State Event
	public static event Action OnLobby;
	public static event Action OnStart;
	public static event Action OnPlaying;
	public static event Action OnNonPauseUI;
	public static event Action OnPauseUI;
	public static event Action OnEnd;

	#endregion


	private void Awake()
	{
		PoolManager.Instance = new PoolManager();
		foreach (var obj in poollistSO.PoolObjectLists)
		{
			PoolManager.Instance.CreatePool(obj, this.transform);
			obj.prefab.pair = obj;
		}
		foreach(var obj in poollistSO.PoolEffectLists)
		{
            PoolManager.Instance.CreatePool(obj, this.transform);
        }
        foreach (var obj in poollistSO.PoolUILists)
        {
            PoolManager.Instance.CreatePool(obj, this.transform);
        }
        if (player == null) player = FindObjectOfType<Player>().GetComponent<Player>();
		if (playerInventory == null) playerInventory = player.GetComponent<ItemInventory>();
	}


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

	#region Methods

	#region Item Manage
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
	#endregion

	public void UpdateState(GameStates SetState)
	{
		gameState = SetState;
		StateChecker();
	}

	private void StateChecker()
	{
		switch (gameState)
		{
			case GameStates.Lobby:
				OnLobby?.Invoke();
				UpdateResult(GameResults.Play);
				break;
			case GameStates.Start:
				OnStart?.Invoke();
				player.ActiveMove = true;
				break;
			case GameStates.Playing:
				OnPlaying?.Invoke();
				player.ActiveMove = true;
				break;
			case GameStates.NonPauseUIOn:
				OnNonPauseUI?.Invoke();
				player.ActiveMove = false;
				break;
			case GameStates.PauseUIOn:
				OnPauseUI?.Invoke();
				player.ActiveMove = false;
				break;
			case GameStates.End:
				OnEnd?.Invoke();
				player.ActiveMove = false;
				break;
			default:
				Debug.LogError("Game Manager Have not State");
				break;
		}
	}

	public void UpdateResult(GameResults SetResult)
	{
		gameResult = SetResult;
		if (gameResult != GameResults.Play) UpdateState(GameStates.End);
	}

	#endregion

}