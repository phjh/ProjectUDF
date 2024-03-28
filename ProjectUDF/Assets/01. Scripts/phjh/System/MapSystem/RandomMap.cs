using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomMap : MonoBehaviour
{
    [SerializeField]
    private List<MapInfoSO> floors;

    [SerializeField]
    private ParticleSystem dirtEffect;

    private GameObject nowMap;

    public int nowFloor = 0;
    public int nowRoom = 0;
    public int nowWave = 0;
    public int leftMonsters = 0;
    public float roomStartTime = 0;

    private void Start()
    {
        floors[nowFloor] = floors[nowFloor].CloneAndSetting();
        nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab);
        dirtEffect.Play();
        MapSystem.Instance.ActionInvoker(MapEvents.MapStart);
        MapSystem.Instance.ActionInvoker(MapEvents.WaveClear);
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            roomStartTime -= 10;
        }
        if (Time.time - roomStartTime > floors[nowFloor].floorRoomInfo[nowRoom].timeLimit)
        {
            Debug.Log("Time over");
        }
        else
        {
            float spawnRate = Time.time - roomStartTime - 40f;
            var emission = dirtEffect.emission;
            emission.rateOverTime = Mathf.Lerp(0, 20f, spawnRate / 40f);
        }
    }

    private void OnEnable()
    {
        MapSystem.Instance.FloorClearEvent += StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent += SetNextMonsterWaves;
        //MapSystem.Instance.MapStartEvent += GameManager.Instance.ReloadStats;
        MapSystem.Instance.MapStartEvent += RoomTimerInit;
        MapSystem.Instance.MapStartEvent += RoomEffectInit;
        MapSystem.Instance.MonsterKilledEvent += MobKilledEvent;
    }

    private void OnDisable()
    {
        MapSystem.Instance.FloorClearEvent -= StageGenerate;
        MapSystem.Instance.MonsterWaveClearEvent -= SetNextMonsterWaves;
        //MapSystem.Instance.MapStartEvent -= GameManager.Instance.ReloadStats;
        MapSystem.Instance.MapStartEvent -= RoomTimerInit;
        MapSystem.Instance.MapStartEvent -= RoomEffectInit;
        MapSystem.Instance.MonsterKilledEvent -= MobKilledEvent;
    }

    //몬스터 소환하는 메서드
    void SpawnMonsters()
    {
        MapInfoSO nowFloor = floors[this.nowFloor];
        leftMonsters = nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave];

        int i = 0;
        foreach(var monsters in nowFloor.floorRoomInfo[nowRoom].spawnMonsters)
        {
            if (monsters.monsterObj.TryGetComponent<PoolableObjectTest>(out PoolableObjectTest obj))
                obj.CustomInstantiate(monsters.monsterPos,obj.poolingType);
            else
            {
                Debug.LogWarning(monsters.monsterObj.name + $"({monsters.monsterObj.GetInstanceID()})" + "was not spawned");
                nowWave++;
            }
             
            Debug.Log($"i : {i + 1}, monsterpos : {monsters.monsterObj.transform.position}");
            //스폰 정보 없애기
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
        SpawnMonsters();
    }

    //탈출구 랜덤스폰 
    void RandomExitSpawn()
    {
        

    }

    void RoomTimerInit()
    {
        TimeManager.Instance.NowTime = floors[nowFloor].floorRoomInfo[nowRoom].timeLimit;
        TimeManager.Instance.StartTimer();
    }

    void RoomEffectInit()
    {
        dirtEffect.Stop();
        roomStartTime = Time.time;
        var em = dirtEffect.emission;
        em.rateOverTime = 0;
        dirtEffect.Play();
    }

    

    //층 마다 생성
    void StageGenerate()
    {
        MapInfoSO newMap = floors[0].CloneAndSettingRandom();
        floors.Add(newMap);
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
