using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	#region Pool
	[Header("Pool Managing Values")]
    [SerializeField]
    private PoolingListSO poollistSO;
    [SerializeField]
    private Transform _poolingTrm;
	#endregion

	public Player player;

    private void Awake()
    {
        PoolManager.Instance = new PoolManager(_poolingTrm);
        foreach (var obj in poollistSO.list)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }

        if(player == null)
        {
            player = FindObjectOfType<Player>().GetComponent<Player>();
        }
    }

}
