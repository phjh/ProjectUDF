using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjectTest : PoolableMono
{
    public override void ResetPooingItem()
    {

    }

    public void CustomInstantiate(Vector2 pos)
    {
        PoolManager.Instance.Pop(PoolingType.Monster);
        transform.position = pos;
    }
}
