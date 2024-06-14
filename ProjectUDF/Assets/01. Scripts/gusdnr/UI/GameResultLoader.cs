using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultLoader : MonoBehaviour
{
    private GameResultData ResultData { get; set;  } = new GameResultData();

    public void GetResultDataInGameManager()
    {
        ResultData = new GameResultData();
        ResultData = GameManager.Instance.resultData;
    }
}
