using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [SerializeField]
    List<MapInfoSO> floors;

    public int nowFloor = 0;
    public int nowRoom = 0;
    public int nowWave = 0;
    public int leftMonsters = 0;

    private void Start()
    {
        floors[nowFloor] = floors[nowFloor].CloneAndSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            floors[nowFloor].floorRoomInfo[0].DebugMonsters();
            SetNextMonsterWaves();
        }
    }

    private void OnEnable()
    {
        MapSystem.Instance.FloorClearEvent += StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent += SetNextMonsterWaves;
    }

    private void OnDisable()
    {
        MapSystem.Instance.FloorClearEvent -= StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent -= SetNextMonsterWaves;
    }

    //���� ��ȯ�ϴ� �޼���
    void SpawnMonsters()
    {
        Dictionary<int, Vector2> spawnPos = new();
        MapInfoSO nowFloor = floors[this.nowFloor];
        leftMonsters = nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave];
        while (spawnPos.Count <= nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave] + 2)
        {
            spawnPos.Add(spawnPos.Count, new Vector2(Random.Range(0, 25), Random.Range(0, 25)));
        }

        Debug.Log($"spawnPos Count : {spawnPos.Count},   monsters count : {nowFloor.floorRoomInfo[nowRoom].spawnMonster.Count} ");

        int i = 0;
        foreach(var monsters in nowFloor.floorRoomInfo[nowRoom].spawnMonster)
        {
            //if(!monsters.TryGetComponent<PoolableObjectTest>(out PoolableObjectTest obj2))
            //{
            //    Debug.LogWarning($"obj : {obj2.name}");
            //}
            PoolableObjectTest obj = monsters.GetComponent<PoolableObjectTest>();
            obj.CustomInstantiate(spawnPos[i],obj.poolingType);
            Debug.Log($"i : {i + 1}, monsterpos : {monsters.transform.position}");
            //���� ���� ���ֱ�
            spawnPos.Remove(i);
            i++;
            if(i >= leftMonsters)
                break;

            //���� ���⼭ ���̺꺸�� ���� ������ break
        }
        nowWave++;

    }


    //���� ���̺� Ŭ���� ����
    void SetNextMonsterWaves()
    {
        SpawnMonsters();
        if(nowWave == floors[nowFloor].floorRoomInfo[nowRoom].monsterWaves)
        {
            nowRoom++;
            MapSystem.Instance.ActionInvoker(4);
            nowWave = 0;
        }
        if(nowRoom == floors[nowFloor].floorRoomInfo.Count)
        {
            nowFloor++;
            MapSystem.Instance.ActionInvoker(2);
            nowRoom = 0;
        }
    }

    //Ż�ⱸ �������� 
    void RandomExitSpawn()
    {
        

    }

    //�� ���� ����
    void StageGenerate()
    {
        MapInfoSO newMap = floors[0].CloneAndSetting();
        floors.Add(newMap);
    }


}
