using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectPoolingType
{

    ChargeAttackEffect,
    ItemGettingEffect,
}

public abstract class EffectPoolableMono : MonoBehaviour
{
    public EffectPoolingType poolingType;
    public abstract void ResetPooingItem();
}

class EffectSystem<T> where T : EffectPoolableMono
{
    private Stack<T> _pool = new Stack<T>();
    private T _prefab; //오리지날 저장
    private Transform _parent;
    private EffectPoolingType _poolingType;

    public EffectSystem(T prefab, EffectPoolingType poolingType, Transform parent, int count = 10)
    {
        _prefab = prefab;
        _parent = parent;
        _poolingType = poolingType;

        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.poolingType = _poolingType;
            obj.gameObject.name = _poolingType.ToString();
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }

    public T Pop()
    {
        T obj = null;
        if (_pool.Count <= 0)
        {
            obj = GameObject.Instantiate(_prefab, _parent);
            obj.gameObject.name = _poolingType.ToString();
            obj.poolingType = _poolingType;
        }
        else
        {
            obj = _pool.Pop();
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }
}