using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    public PoolingType poolingType;
    public abstract void ResetPooingItem();

    public void CustomInstantiate(Vector2 pos, PoolingType type)
    {
        PoolableMono poolItem = PoolManager.Instance.Pop(type);
        poolItem.transform.position = pos;
    }
}