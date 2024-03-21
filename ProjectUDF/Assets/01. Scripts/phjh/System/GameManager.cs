using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PoolingListSO poollistSO;

    [SerializeField]
    Transform _poolingTrm;

    private void Awake()
    {
        PoolManager.Instance = new PoolManager(_poolingTrm);
        foreach (var obj in poollistSO.list)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }
    }

}
