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
        if (Input.GetKeyDown(KeyCode.L))
        {
            MobKilledEvent();
        }
    }

    private void OnEnable()
    {
        MapSystem.Instance.FloorClearEvent += StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent += SetNextMonsterWaves;
        MapSystem.Instance.MapStartEvent += ReloadStats;
        MapSystem.Instance.MonsterKilledEvent += MobKilledEvent;
    }

    private void OnDisable()
    {
        MapSystem.Instance.FloorClearEvent -= StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent -= SetNextMonsterWaves;
        MapSystem.Instance.MapStartEvent -= ReloadStats;
        MapSystem.Instance.MonsterKilledEvent -= MobKilledEvent;
    }

    //몬스터 소환하는 메서드
    void SpawnMonsters()
    {
        Dictionary<int, Vector2> spawnPos = new();
        MapInfoSO nowFloor = floors[this.nowFloor];
        leftMonsters = nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave];
        while (spawnPos.Count <= nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave] + 2)
        {
            spawnPos.Add(spawnPos.Count, new Vector2(Random.Range(0, 10), Random.Range(0, 10)));
        }
             
        Debug.Log($"spawnPos Count : {spawnPos.Count},   monsters count : {nowFloor.floorRoomInfo[nowRoom].spawnMonster.Count} ");

        int i = 0;
        foreach(var monsters in nowFloor.floorRoomInfo[nowRoom].spawnMonster)
        {
            if (monsters.TryGetComponent<PoolableObjectTest>(out PoolableObjectTest obj))
                obj.CustomInstantiate(spawnPos[i],obj.poolingType);
            else
            {
                Debug.LogWarning(monsters.name + $"({monsters.GetInstanceID()})" + "was not spawned");
                nowWave++;
            }
             
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
            MapSystem.Instance.ActionInvoker(MapEvents.MapClear);
            nowRoom++;
            nowWave = 0;
            Destroy(nowMap.gameObject);
            if (nowRoom != floors[nowFloor].floorRoomInfo.Count)
                nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab);
            MapSystem.Instance.ActionInvoker(MapEvents.MapStart);
        }
        if (nowRoom == floors[nowFloor].floorRoomInfo.Count)
        {
            MapSystem.Instance.ActionInvoker(MapEvents.FloorClear);
            nowFloor++;
            nowRoom = 0;
            MapSystem.Instance.ActionInvoker(MapEvents.FloorStart);

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

    void ReloadStats()
    {
        GameManager.Instance.Lucky = GameManager.Instance.player._playerStat.Lucky.GetValue();
        GameManager.Instance.Strength = GameManager.Instance.player._playerStat.Strength.GetValue();
        GameManager.Instance.MoveSpeed = GameManager.Instance.player._playerStat.MoveSpeed.GetValue();
        GameManager.Instance.AttackSpeed = GameManager.Instance.player._playerStat.AttackSpeed.GetValue();
    }

    void MobKilledEvent()
    {
        leftMonsters--;
        if(leftMonsters == 0)
        {
            MapSystem.Instance.ActionInvoker(MapEvents.WaveClear);
        }
    }

}
