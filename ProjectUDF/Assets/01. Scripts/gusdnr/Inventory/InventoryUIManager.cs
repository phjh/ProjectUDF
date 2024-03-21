using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject OrePrefab;
    [SerializeField] private RectTransform OreParent;
    
    public List<GameObject> OrePrefabList;

    private void AddOreIcon()
    {
        GameObject newOre = Instantiate(OrePrefab);
        newOre.transform.parent = transform;
        newOre.transform.localPosition = Vector3.zero;
    }

    public void Show()
    {

    }

    public void Close()
    {

    }
}
