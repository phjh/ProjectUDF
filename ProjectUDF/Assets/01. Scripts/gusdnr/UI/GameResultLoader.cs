using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultLoader : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private string DieText;
    [SerializeField] private string TimeOutText;
    [SerializeField] private string ClearText;

    [Header("UI Objects")]
    [SerializeField] private TextMeshProUGUI ResultText;
    [SerializeField] private TextMeshProUGUI ClearRoomCountText;
    [SerializeField] private Button ReStartButton;
    [SerializeField] private Button QuitButton;

    private GameResultData ResultData { get; set;  } = new GameResultData();

	private void OnEnable()
	{
		InGameSceneManager.OnStartLoadScene += GetResultDataInGameManager;
	}

	private void OnDisable()
	{
		InGameSceneManager.OnStartLoadScene -= GetResultDataInGameManager;
	}

    private void Start()
    {
        ResultData = GameResult.Instance.resultData;
        SetResultUI();
    }

    public void GetResultDataInGameManager()
    {
        //ResultData = new GameResultData();
        //ResultData = GameManager.Instance.ReturnGameResultData();

        if(ResultData != null) SetResultUI();
	}

    private void SetResultUI()
    {
        if(ResultData == null)
        {
            Debug.LogWarning("Result Data is Null [Error: Null Ref]");
            return;
        }

		ResultText.text = ConvertResultStateToString();
        ClearRoomCountText.text = ConvertClearRoomCount(ResultData.ClearRoomCount);

        ReStartButton.onClick.RemoveAllListeners();
        QuitButton.onClick.RemoveAllListeners();

        ReStartButton.onClick.AddListener(() => InGameSceneManager.Instance.SetSceneSceneList(SceneList.Start));
        QuitButton.onClick.AddListener(() => InGameSceneManager.Instance.QuitGame());
	}

    private string ConvertResultStateToString()
    {
        switch(ResultData.ResultState)
        {
            case GameResults.None:
            case GameResults.Playing:
                break;

            case GameResults.DiePlayer:
            {
                ResultText.color = Color.red;
                return DieText;
            }
            case GameResults.TimeOut:
            {
                ResultText.color = Color.red;
                return TimeOutText;
            }
            case GameResults.ClearGame:
            {
                ResultText.color = Color.blue;
                return ClearText;
            }
            
            default: return null;
		}

        return null;
    }

    private string ConvertClearRoomCount(int count)
    {
        string uiText = (count/10).ToString() + (count%10).ToString();
        uiText.Replace("count", count.ToString());

        return uiText;
    }
}
