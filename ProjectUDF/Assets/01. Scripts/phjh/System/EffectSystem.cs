using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectPoolingType
{
    LeftAttackEffect,
    ChargeAttackEffect,
    ChargeAttackEffect2,
    ItemGettingEffect,
}

public abstract class EffectPoolableMono : MonoBehaviour
{
    public EffectPoolingType poolingType;
    public abstract void ResetPooingItem();
}

class EffectPool<T> where T : EffectPoolableMono
{
    private Stack<T> _pool = new Stack<T>();
    private T _prefab; //오리지날 저장
    private Transform _parent;
    private EffectPoolingType _poolingType;

    public EffectPool(T prefab, EffectPoolingType poolingType, Transform parent, int count = 10)
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

public class EffectSystem : MonoSingleton<EffectSystem>
{
    public void EffectInvoker(EffectPoolingType type, Vector3 targetPos, float waitDuration) => StartCoroutine(EffectInvoke(type, targetPos, waitDuration));
    public void EffectInvoker(EffectPoolingType type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform) => StartCoroutine(EffectInvoke(type, targetPos, waitDuration, rot, rotTransform));

    private IEnumerator EffectInvoke(EffectPoolingType type, Vector3 targetPos, float waitDuration)
    {
        EffectPoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem);
    }

    private IEnumerator EffectInvoke(EffectPoolingType type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform)
    {
        EffectPoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        poolItem.transform.rotation = Quaternion.Euler(0,0,rot);
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem);
    }
}