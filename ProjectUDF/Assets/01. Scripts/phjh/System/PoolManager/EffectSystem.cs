using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EffectSystem : MonoSingleton<EffectSystem>
{
    public void EffectInvoker(PoolingPair pair, Vector3 targetPos, float waitDuration) => StartCoroutine(EffectInvoke(pair, targetPos, waitDuration));
    public void EffectInvoker(PoolingPair pair, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform) => StartCoroutine(EffectInvoke(pair, targetPos, waitDuration, rot, rotTransform));
    //public void EffectInvoker(PoolingObjectType type, string name, Vector3 targetPos, float waitDuration) => StartCoroutine(EffectInvoke(pair, targetPos, waitDuration));
    //public void EffectInvoker(PoolingObjectType type, int ID, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform) => StartCoroutine(EffectInvoke(pair, targetPos, waitDuration, rot, rotTransform));

    private IEnumerator EffectInvoke(PoolingPair pair, Vector3 targetPos, float waitDuration)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(pair);
        poolItem.transform.position = targetPos;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(pair);
    }

    private IEnumerator EffectInvoke(PoolingPair pair, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(pair);
        poolItem.transform.position = targetPos;
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.transform.rotation = Quaternion.Euler(0, 0, rot);
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(pair);
    }
    private IEnumerator EffectInvoke(string name, Vector3 targetPos, float waitDuration)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(name);
        poolItem.transform.position = targetPos;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, name);
    }

    private IEnumerator EffectInvoke(string name, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(name);
        poolItem.transform.position = targetPos;
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.transform.rotation = Quaternion.Euler(0, 0, rot);
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, name);
    }
}