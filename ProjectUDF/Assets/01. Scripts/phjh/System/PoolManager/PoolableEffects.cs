using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableEffects : EffectPoolableMono
{
    public override void ResetPooingItem()
    {

    }

    public void CustomInstantiate(Vector2 pos, EffectPoolingType type)
    {
        EffectPoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = pos;
    }
}
