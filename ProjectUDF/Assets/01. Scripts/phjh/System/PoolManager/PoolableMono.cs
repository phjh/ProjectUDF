using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    [HideInInspector]
    public PoolingPair pair;

    private void Start()
    {
        pair.prefab = this.gameObject.GetComponent<PoolableMono>();
    }

    public abstract void ResetPooingItem(); //init Pooling Items

    public void CustomInstantiate(Vector2 pos, PoolingPair pair)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(pair);
        poolItem.transform.position = pos;
    }

    public void CustomInstantiate(Vector2 pos, string name = "", int ID = 0)
    {
        PoolableMono poolItem;
        if(name == "")
        {
            poolItem = PoolManager.Instance.Pop(ID);
        }
        else
        {
            poolItem = PoolManager.Instance.Pop(name);
        }
        poolItem.transform.position = pos;
    }
}