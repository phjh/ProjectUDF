using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public EnemyMain[] EnemyList;
    
    public void SettingEnemyList()
    {
        EnemyList = GetComponentsInChildren<EnemyMain>();
        foreach(EnemyMain enemy in EnemyList)
        {
            Debug.Log("�� ����Ʈ�� �޴� ��");
        }
    }
}
