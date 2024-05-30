using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    [HideInInspector]
    public PoolingPair pair;

    private void Start()
    {
        pair.prefab = this.gameObject.GetComponent<PoolableMono>();
    }

    public abstract void ResetPoolingItem(); //init Pooling Items

    public void CustomInstantiate(Vector2 pos, PoolObjectListEnum objenum)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(objenum);
        poolItem.transform.localPosition = pos;
    }



}
