using GameManageDefine;
using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	#region Pooling
	[Header("Pooling")]
	public PoolingListSO poollistSO;
	#endregion

	#region Player Info
	[Header("Player")]
	public PlayerMain player;
	[Tooltip("ī�޶�")]
	public CinemachineVirtualCamera VirtualCam;
	#endregion

	#region In Game Flow
	public GameStates gameState;
	public GameResults gameResult;
	#endregion

	#region Game State Event
	public static event Action OnLobby;
	public void LobbyEventHandler() { if (OnLobby != null) OnLobby.Invoke(); }

	public static event Action OnStart;
	public void StartEventHandler() { if (OnStart != null) OnStart.Invoke(); }

	public static event Action OnPlaying;
	public static void PlayingEventHandler() { if (OnPlaying != null) OnPlaying.Invoke(); }

	public static event Action OnNonPauseUI;
	public void NonPauseEventHandler() { if (OnNonPauseUI != null) OnNonPauseUI.Invoke(); }

	public static event Action OnPauseUI;
	public void PauseEventHandler() { if (OnPauseUI != null) OnPauseUI.Invoke(); }

	public static event Action OnEnd;
	public void EndEventHandler() { if (OnEnd != null) OnEnd.Invoke(); }
	#endregion

	#region etc

	//ī�޶� ���?���̴� ��
	CinemachineBasicMultiChannelPerlin perlin;

	#endregion

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Awake()
	{
		SetPoolManager();
		if (player == null) player = PlayerMain.Instance;

        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		

		UpdateResult(GameResults.None);
	}

	private void Start()
	{
		AstarPath.active.Scan();
		player.DeadEvent += SetDeadInfo; 
	}

	#region Methods

	void SetDeadInfo()
	{
		GameResult.Instance.result = gameResult;
		GameResult.Instance.clearRoomCount = MapSystem.Instance.ClearRoomCount;
	}

	public void SetPoolManager()
	{
        PoolManager.Instance = new PoolManager();

		#region Ǯ�Ŵ��� �θ� ����

		GameObject PoolingParent = new GameObject()
;        PoolingParent.name = "PoolingObjectParent";

		List<GameObject> categoryParent = new();

		categoryParent.Add(new GameObject());
		categoryParent[categoryParent.Count - 1].transform.SetParent(PoolingParent.transform);
		categoryParent[categoryParent.Count - 1].name = "ObjectsList";

        categoryParent.Add(new GameObject());
        categoryParent[categoryParent.Count - 1].transform.SetParent(PoolingParent.transform);
        categoryParent[categoryParent.Count - 1].name = "EffectsList";

        categoryParent.Add(new GameObject());
        categoryParent[categoryParent.Count - 1].transform.SetParent(PoolingParent.transform);
        categoryParent[categoryParent.Count - 1].name = "UILists";

		#endregion

		foreach (var obj in poollistSO.PoolObjectLists)
		{
			PoolManager.Instance.CreatePool(obj, categoryParent[0].transform);
			obj.prefab.pair.enumtype = obj.enumtype;
			obj.prefab.pair = obj;
		}
		foreach (var obj in poollistSO.PoolEffectLists)
		{
			PoolManager.Instance.CreatePool(obj, categoryParent[1].transform);
		}
		foreach (var obj in poollistSO.PoolUILists)
		{
			PoolManager.Instance.CreatePool(obj, categoryParent[2].transform);
		}
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
			case GameStates.Lobby:
				OnLobby?.Invoke();
				UpdateResult(GameResults.Playing);
				break;
			case GameStates.Start:
				OnStart?.Invoke();
				AstarPath.active.Scan();
				player.canMove = true;
				break;
			case GameStates.Playing:
				OnPlaying?.Invoke();
				player.canMove = true;
				break;
			case GameStates.NonPauseUIOn:
				OnNonPauseUI?.Invoke();
				player.canMove = false;
				break;
			case GameStates.PauseUIOn:
				OnPauseUI?.Invoke();
				player.canMove = false;
				break;
			case GameStates.End:
				OnEnd?.Invoke();
				player.canMove = false;
				break;
			default:
				Debug.LogError("Game Manager Have not EnemyMotionState");
				break;
		}
	}

	public void UpdateResult(GameResults SetResult)
	{
		gameResult = SetResult;

		if (gameResult != GameResults.Playing && gameResult != GameResults.None) UpdateState(GameStates.End);
	}

	public GameResultData ReturnGameResultData()
	{
		List<int> getOreList = OreInventory.Instance?.OreList;
		List<int> getGemList = OreInventory.Instance?.GemList;
		
		GameResultData PackedResultData = new GameResultData()
		{
			ResultState = gameResult,
			ClearRoomCount = MapSystem.Instance.ClearRoomCount,
			CollectOres = getOreList,
			CollectGems = getGemList
		};

		return PackedResultData;
	}

    #endregion

    #region CameraShake

    public void ShakeCamera(float shakeIntencity = 3, float waitTime = 0.2f)
    {
        float frequency = 1f;
        if (PlayerMain.Instance.isCritical)
        {
            shakeIntencity *= 1.2f;
            frequency += 0.5f;
        }
        perlin.m_AmplitudeGain = shakeIntencity * 0.5f;
        perlin.m_FrequencyGain = frequency;
		Invoke(nameof(CameraShakingOff), waitTime);
    }

    void CameraShakingOff()
    {
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }

    #endregion

}