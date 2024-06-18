using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoSingleton<GameResult>
{
    public GameResultData resultData;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void DeleteThis()
    {
        Destroy(gameObject);
    }

}
