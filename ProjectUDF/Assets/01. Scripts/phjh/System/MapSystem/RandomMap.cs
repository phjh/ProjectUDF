using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RandomMap : MonoBehaviour
{
    [SerializeField]
    List<MapInfoSO> floors;

    [SerializeField]
    NavGraph groundScan;

    private GameObject nowMap;

    public int nowFloor = 0;
    public int nowRoom = 0;
    public int nowWave = 0;
    public int leftMonsters = 0;

    private void Start()
    {
        floors[nowFloor] = floors[nowFloor].CloneAndSettingRandom();
        nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab);
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

    //몬스터 소환하는 메서드
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
            if (monsters.TryGetComponent<PoolableObjectTest>(out PoolableObjectTest obj))
                obj.CustomInstantiate(spawnPos[i],obj.poolingType);
            else
                Debug.LogError($"obj : nul, {monsters.name}");
            Debug.Log($"i : {i + 1}, monsterpos : {monsters.transform.position}");
            //스폰 정보 없애기
            spawnPos.Remove(i);
            i++;
            if(i >= leftMonsters)
                break;

            //대충 여기서 웨이브보다 많이 스폰시 break
        }
        nowWave++;
    }


    //몬스터 웨이브 클리어 세팅
    void SetNextMonsterWaves()
    {
        SpawnMonsters();
        if(nowWave == floors[nowFloor].floorRoomInfo[nowRoom].monsterWaves)
        {
            nowRoom++;
            MapSystem.Instance.ActionInvoker(4);
            nowWave = 0;
            Destroy(nowMap.gameObject);
            if (nowRoom != floors[nowFloor].floorRoomInfo.Count)
                nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab);
        }
        if (nowRoom == floors[nowFloor].floorRoomInfo.Count)
        {
            nowFloor++;
            MapSystem.Instance.ActionInvoker(2);
            nowRoom = 0;

        }
        if (groundScan != null)
            groundScan.Scan();
    }

    //탈출구 랜덤스폰 
    void RandomExitSpawn()
    {
        

    }

    //층 마다 생성
    void StageGenerate()
    {
        MapInfoSO newMap = floors[0].CloneAndSettingRandom();
        floors.Add(newMap);
    }


}
