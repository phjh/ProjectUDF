﻿using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    //[HideInInspector]
    public PoolingPair pair;

    private void Start()
    {
        pair.prefab = this.gameObject.GetComponent<PoolableMono>();
    }

    public abstract void ResetPoolingItem(); //init Pooling Items

    public void CustomInstantiate(Vector2 pos, PoolObjectListEnum objenum)
    {
        Debug.Log("CustomInstantiate : " + objenum);
        PoolableMono poolItem = PoolManager.Instance.Pop(objenum);
        poolItem.transform.localPosition = pos;
    }
	public PoolableMono InstantiateReturnObject(Vector2 pos, PoolObjectListEnum objenum)
	{
		PoolableMono poolItem = PoolManager.Instance.Pop(objenum);
		poolItem.transform.localPosition = pos;
        return poolItem;
	}



}
