using System.Collections;
using UnityEngine;

class EffectSystem : MonoSingleton<EffectSystem>
{
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration, Transform parent = null) => EffectInvoke(pair.enumtype, targetPos, waitDuration, parent);
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform, Transform parent = null) 
        => EffectInvoke(pair.enumtype, targetPos, waitDuration, rot, rotTransform, parent);
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null) => EffectInvoke(type, targetPos, waitDuration);

    public void EffectsInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null) => EffectsInvoke(type, targetPos, waitDuration);
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform, Transform parent = null) 
        => EffectInvoke(type, targetPos, waitDuration, rot, rotTransform, parent);

    private void EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null)
    {
        PoolableMono poolItem = BasePop(type, targetPos,waitDuration, parent);
        poolItem.GetComponent<ParticleSystem>().Play();
    }

    private void EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform,Transform parent = null)
    {
        PoolableMono poolItem = BasePop(type, targetPos,waitDuration, parent);
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.transform.rotation = Quaternion.Euler(0, 0, rot);
        poolItem.GetComponent<ParticleSystem>().Play();
    }

    private void EffectsInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null)
    {
        PoolableMono poolItem = BasePop(type, targetPos, waitDuration, parent);
        foreach (var particles in poolItem.GetComponentsInChildren<ParticleSystem>())
        {
            particles.Play(); 
        }
    }

    public PoolableMono BasePop(PoolEffectListEnum type,Vector3 spawnPos,float waitDuration, Transform parent = null)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = spawnPos;
        if (parent != null)
            poolItem.transform.SetParent(parent);
        StartCoroutine(BasePush(poolItem, type, waitDuration));
        return poolItem;
    }

    IEnumerator BasePush(PoolableMono mono, PoolEffectListEnum type, float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Push(mono, type);
    }

}