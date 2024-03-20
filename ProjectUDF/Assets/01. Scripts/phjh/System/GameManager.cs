using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PoolingListSO poollistSO;

    private void Awake()
    {
        
        foreach(var obj in poollistSO.list)
        {

        }

    }

}
