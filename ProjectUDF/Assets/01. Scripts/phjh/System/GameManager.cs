using GameManageDefine;
using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public GameResultData resultData
	{
		get
		{
			return resultData;
		}

		private set
		{
			resultData.ResultState = value.ResultState;
			resultData.ClearRoomCount = value.ClearRoomCount;

			resultData.CollectOres = value.CollectOres;
			resultData.CollectGems = value.CollectGems;
		}
	}

	#region Pooling
	[Header("Pooling")]
	public PoolingListSO poollistSO;
	#endregion

	#region Player Info
	[Header("Player")]
	public PlayerMain player;
	[Tooltip("카메라")]
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

	//카메라 흔들때 쓰이는 거
	CinemachineBasicMultiChannelPerlin perlin;

	#endregion

	private void OnEnable()
	{
		OnEnd += SetResultData;
	}

	private void OnDisable()
	{
		OnEnd -= SetResultData;
	}

	private void Awake()
	{
		SetPoolManager();
		if (player == null) player = PlayerMain.Instance;

        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		AstarPath.active.Scan();

		UpdateResult(GameResults.None);
		resultData = new GameResultData() { ClearRoomCount = 0, ResultState = gameResult, CollectOres = new List<int>(), CollectGems = new List<int>() };
	}

	#region Methods

	public void SetPoolManager()
	{
        PoolManager.Instance = new PoolManager();

		#region 풀매니저 부모 세팅

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
				SetResultData();
				player.canMove = false;
				break;
			default:
				Debug.LogError("Game Manager Have not EnemyMotionState");
				break;
		}
	}

	public void UpdateResult(GameResults SetResult)
	{
		resultData.ResultState = SetResult;

		if (resultData.ResultState != GameResults.Playing && resultData.ResultState != GameResults.None) UpdateState(GameStates.End);
	}

	public void SetResultData()
	{
		resultData.ClearRoomCount = MapSystem.Instance.ClearRoomCount;

		List<int> oreList = OreInventory.Instance?.OreList;
		List<int> gemList = OreInventory.Instance?.GemList;

		if(oreList != null)	resultData.CollectOres = oreList;
		if(gemList != null)	resultData.CollectGems = gemList;
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