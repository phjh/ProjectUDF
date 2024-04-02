using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager 
{
    public static PoolManager Instance;

    [SerializeField]
    private Dictionary<PoolingType, Pool<PoolableMono>> _pools = new Dictionary<PoolingType, Pool<PoolableMono>>();
    [SerializeField]
    private Dictionary<EffectPoolingType, EffectPool<EffectPoolableMono>> _effectPools = new Dictionary<EffectPoolingType, EffectPool<EffectPoolableMono>>();

    private Transform _trmParent;
    public PoolManager(Transform trmParent)
    {
        _trmParent = trmParent;
    }

    public void CreatePool(PoolableMono prefab, PoolingType poolingType, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, _trmParent, count);
        Debug.Log('a');
        _pools.Add(poolingType, pool);
    }

    public PoolableMono Pop(PoolingType type)
    {
        if (!_pools.ContainsKey(type))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = _pools[type].Pop();
        item.ResetPooingItem();
        return item;
    }

    public void Push(PoolableMono obj, bool resetParent = false)
    {
        if(resetParent)
            obj.transform.parent = _trmParent;
        _pools[obj.poolingType].Push(obj);
    }

    public void CreatePool(EffectPoolableMono prefab, EffectPoolingType poolingType, int count = 10)
    {
        EffectPool<EffectPoolableMono> pool = new EffectPool<EffectPoolableMono>(prefab, poolingType, _trmParent, count);
        _effectPools.Add(poolingType, pool);
    }

    public EffectPoolableMono Pop(EffectPoolingType type)
    {
        if (!_effectPools.ContainsKey(type))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        EffectPoolableMono item = _effectPools[type].Pop();
        item.ResetPooingItem();
        return item;
    }


    public void Push(EffectPoolableMono obj, bool resetParent = false)
    {
        if (resetParent)
            obj.transform.parent = _trmParent;
        _effectPools[obj.poolingType].Push(obj);
    }
}