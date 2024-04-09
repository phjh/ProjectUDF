using System.Collections;
using UnityEngine;

class EffectSystem : MonoSingleton<EffectSystem>
{
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration) => StartCoroutine(EffectInvoke(pair.enumtype, targetPos, waitDuration));
    public void EffectInvoker(EffectPoolingPair pair, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform) => StartCoroutine(EffectInvoke(pair.enumtype, targetPos, waitDuration, rot, rotTransform));
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration) => StartCoroutine(EffectInvoke(type, targetPos, waitDuration));
    public void EffectInvoker(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform) => StartCoroutine(EffectInvoke(type, targetPos, waitDuration, rot, rotTransform));

    private IEnumerator EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, type);
    }

    private IEnumerator EffectInvoke(PoolEffectListEnum type, Vector3 targetPos, float waitDuration, float rot, Vector3 rotTransform)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetPos;
        poolItem.transform.position += Quaternion.Euler(0, 0, rot) * rotTransform;
        poolItem.transform.rotation = Quaternion.Euler(0, 0, rot);
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem, type);
    }
}