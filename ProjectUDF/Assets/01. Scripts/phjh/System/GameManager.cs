using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    [SerializeField]
    PoolingListSO poollistSO;

    [SerializeField]
    Transform _poolingTrm;

    public float Strength;
    public float Lucky;
    public float AttackSpeed;
    public float MoveSpeed;

    private void Awake()
    {
        PoolManager.Instance = new PoolManager(_poolingTrm);
        foreach (var obj in poollistSO.list)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }
    }

}
