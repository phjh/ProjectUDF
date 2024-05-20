using Cinemachine;
using System;
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
	public static event Action OnStart;
	public static event Action OnPlaying;
	public static event Action OnNonPauseUI;
	public static event Action OnPauseUI;
	public static event Action OnEnd;
	#endregion

	#region etc

	//카메라 흔들때 쓰이는 거
	CinemachineBasicMultiChannelPerlin perlin;

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
		if (player == null) player = PlayerMain.Instance;

        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        AstarPath.active.Scan();
	}

	#region Methods
	public void UpdateState(GameStates SetState)
	{
		gameState = SetState;
		//StateChecker();
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

    #region CameraShake

    public void ShakeCamera(float shakeIntencity = 3, float waittime = 0.2f)
    {
        float frequency = 1f;
        if (PlayerMain.Instance.isCritical)
        {
            shakeIntencity *= 1.2f;
            frequency += 0.5f;
        }
        perlin.m_AmplitudeGain = shakeIntencity * 0.5f;
        perlin.m_FrequencyGain = frequency;
		Invoke(nameof(CameraShakingOff), waittime);
    }

    void CameraShakingOff()
    {
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }

    #endregion

}