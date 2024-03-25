using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneList
{

}

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
        foreach (var obj in poollistSO.PoolObjtectList)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }
        foreach (var obj in poollistSO.PoolEffectLists)
        {
            PoolManager.Instance.CreatePool(obj.prefab, obj.type, obj.count);
        }
        DontDestroyOnLoad(this);
    }

    public void ReloadStats()
    {
        Lucky = player._playerStat.Lucky.GetValue();
        Strength = player._playerStat.Strength.GetValue();
        MoveSpeed = player._playerStat.MoveSpeed.GetValue();
        AttackSpeed = player._playerStat.AttackSpeed.GetValue();
    }

    public void EffectInvoker(EffectPoolingType type, Transform targetTrm, float waitDuration) => StartCoroutine(EffectInvoke(type, targetTrm, waitDuration));

    private IEnumerator EffectInvoke(EffectPoolingType type,Transform targetTrm, float waitDuration)
    {
        EffectPoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = targetTrm.transform.position;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitDuration);
        PoolManager.Instance.Push(poolItem);
    }
}
