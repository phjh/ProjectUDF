using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvnetoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject OrePrefab;
    
    private void AddOreIcon()
    {
        GameObject newOre = Instantiate(OrePrefab);
        newOre.transform.parent = transform;
        newOre.transform.localPosition = Vector3.zero;
    }

}
