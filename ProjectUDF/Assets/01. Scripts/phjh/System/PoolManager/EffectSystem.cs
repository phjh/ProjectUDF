using System.Collections;
using UnityEngine;

class EffectSystem : MonoSingleton<EffectSystem>
{
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration, Transform parent = null) => StartCoroutine(EffectInvoke(pair.enumtype, targetPos, waitDuration, parent));
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform, Transform parent = null) 
        => StartCoroutine(EffectInvoke(pair.enumtype, targetPos, waitDuration, rot, rotTransform, parent));
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null) => StartCoroutine(EffectInvoke(type, targetPos, waitDuration));

    public void EffectsInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null) => StartCoroutine(EffectsInvoke(type, targetPos, waitDuration));
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform, Transform parent = null) 
        => StartCoroutine(EffectInvoke(type, targetPos, waitDuration, rot, rotTransform, parent));

    private IEnumerator EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        if (parent != null)
            poolItem.transform.SetParent(parent);
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, type);
    }

    private IEnumerator EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform,Transform parent = null)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.transform.rotation = Quaternion.Euler(0, 0, rot);
        if(parent != null)
            poolItem.transform.SetParent(parent);
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, type);
    }

    private IEnumerator EffectsInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, Transform parent = null)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        if (parent != null)
            poolItem.transform.SetParent(parent);
        foreach(var particles in poolItem.GetComponentsInChildren<ParticleSystem>())
        {
            particles.Play(); 
        }
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, type);
    }
}