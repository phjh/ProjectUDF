using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultLoader : MonoBehaviour
{
    [SerializeField] private string DieText;
    [SerializeField] private string TimeOutText;
    [SerializeField] private string ClearText;

    private GameResultData ResultData { get; set;  } = new GameResultData();

	private void OnEnable()
	{
		InGameSceneManager.OnStartLoadScene += GetResultDataInGameManager;
	}

	private void OnDisable()
	{
		InGameSceneManager.OnStartLoadScene -= GetResultDataInGameManager;
	}

	public void GetResultDataInGameManager()
    {
        ResultData = new GameResultData();
        ResultData = GameManager.Instance.resultData;

        if(ResultData != null) SetResultUI();
	}

    private void SetResultUI()
    {
        if(ResultData == null)
        {
            Debug.LogWarning("Result Data is Null [Error: Null Ref]");
            return;
        }

		ConvertResultStateToString();

	}

    private string ConvertResultStateToString()
    {
        switch(ResultData.ResultState)
        {
            case GameResults.None:
            case GameResults.Playing:
                break;

            case GameResults.DiePlayer:
                return DieText;
            case GameResults.TimeOut:
                return TimeOutText;
            case GameResults.ClearGame:
                return ClearText;
            
            default: return null;
		}

        return null;
    }
}
