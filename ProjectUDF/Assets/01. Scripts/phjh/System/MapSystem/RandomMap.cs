using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [SerializeField]
    List<MapInfoSO> floors;

    int nowfloor = 0;

    private void Start()
    {
        floors[nowfloor].CloneAndSetting();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    //���� ���̺� Ŭ���� ����
    void SetNextMonsterWaves()
    {

    }

    void MonsterWaveClear()
    {

    }


    //Ż�ⱸ �������� 
    void RandomExitSpawn()
    {
        

    }



    //�� ���� ����
    void StageGenerate()
    {

    }


}
