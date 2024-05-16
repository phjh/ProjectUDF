using System;
using System.Collections.Generic;
using UnityEngine;

public enum MapEvents
{
    FloorStart = 1,
    FloorClear = 2,
    MapStart =3,
    MapClear = 4,
    WaveClear = 5,
    MonsterKill = 6
}

public class MapSystem : MonoSingleton<MapSystem>
{
    //보스를 제외한 한 층의 정보를 담고있다 (각 방의 정보)
    //public List<MapInfoSO> StageInfo;

    //층 따라 시작되는 이벤트 (삭제 예정)
    public event Action FloorStartEvent; //1번  -> 층을 새작할때 실행된다.  랜덤층 생성이 여기서 이루어졌다.
    //public event Action FloorClearEvent; //2번  ->  각 층을 클리어 했을때 실행된다. 다음층으로 가는 포탈 생성을 여기서 할 예정이다

    //방 마다 시작되는 이벤트
    public event Action RoomStartEvent;  //3번   ->  각 방에 들어갈때 실행된다.  시간제한이 여기 포함된다
    public event Action RoomClearEvent;  //4번  ->   각 방을 클리어 했을때 나온다.  채광같은게 여기 포함된다

    //몬스터 웨이브 깰때마다 시작되는 이벤트
    public Action MonsterWaveClearEvent;  //5번  ->  웨이브에 모든 몬스터를 다 잡았을시 실행된다.  다음 웨이브 몬스터 소환 등을 할때 쓰인다

    public event Action MonsterKilledEvent; //6번 -> 몹이 죽을때마다 실행할 이벤트, 몹이 죽을때 연결해주면 된다.

    public void ActionInvoker(MapEvents e)
    {
        switch ((int)e)
        {
            case 1:
                FloorStartEvent?.Invoke();
                break;
            //case 2:
            //    FloorClearEvent?.Invoke();
                //break;
            case 3:
                RoomStartEvent?.Invoke();
                break;
            case 4:
                RoomClearEvent?.Invoke();
                break;
            case 5:
                MonsterWaveClearEvent?.Invoke();
                break;
            case 6:
                MonsterKilledEvent?.Invoke();
                break;
            default:
                break;
        }
    }

    [SerializeField]
    private List<MapInfoSO> floors;

    [SerializeField]
    private ParticleSystem dirtEffect;

    [SerializeField]
    private List<GameObject> Portals;

    private GameObject nowMap;

    public GameObject ExitPrefab;

    public int nowFloor = 0;
    public int nowRoom = 0;
    public int nowWave = -1;
    public int leftMonsters = 0;
    public float roomStartTime = 0;

    public bool IsRandomExit = false;

    private void Start()
    {
        floors[nowFloor] = floors[nowFloor].CloneAndSetting();      //여기 Random붙이면 됨
        nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab);
        dirtEffect.Play();
        WaveClear();
        SetRoomMap();
        RoomTimerInit();
        RoomEffectInit();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            WaveClear();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnMonsterDead();
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
        MapSystem.Instance.RoomClearEvent += OnRoomClear;
        MapSystem.Instance.RoomStartEvent += OnRoomStart;
        MapSystem.Instance.MonsterWaveClearEvent += WaveClear;
        MapSystem.Instance.MonsterKilledEvent += OnMonsterDead;
    }

    private void OnDisable()
    {
        MapSystem.Instance.RoomClearEvent -= OnRoomClear;
        MapSystem.Instance.RoomStartEvent -= OnRoomStart;
        MapSystem.Instance.MonsterWaveClearEvent -= WaveClear;
        MapSystem.Instance.MonsterKilledEvent -= OnMonsterDead;
    }

    #region GameFlow

    public void OnMonsterDead()
    {
        leftMonsters--;
        if (leftMonsters <= 0)
        {
            WaveClear();
        }
    }

    void WaveClear()
    {
        if (nowWave == floors[nowFloor].floorRoomInfo[nowRoom].monsterWaves - 1)
        {
            nowWave = 0;
            nowRoom++;
            OnRoomClear();
        }
        else
        {
            nowWave++;
            SpawnMonsters();
        }
    }

    public void OnRoomStart()
    {
        RoomTimerInit();
        RoomEffectInit();
        SetRoomMap();
        SetLeftMonsters();
        SpawnMonsters();
    }

    void OnRoomClear()
    {
        PortalSpawn();
        if (nowRoom == floors[nowFloor].floorRoomInfo.Count)
        {
            OnFloorClear();
            nowFloor++;
            nowRoom = 0;
        }
    }

    public void OnFloorStart()
    {

    }

    public void OnFloorClear()
    {
        StageGenerate();
    }

    #endregion

    void SetLeftMonsters() => leftMonsters = floors[nowFloor].floorRoomInfo[nowRoom].numberOfMonsters[0];

    //몬스터 소환하는 메서드
    void SpawnMonsters()
    {
        MapInfoSO nowFloor = floors[this.nowFloor];
        leftMonsters = nowFloor.floorRoomInfo[nowRoom].numberOfMonsters[nowWave];

        int i = 0;
        foreach (var monsters in nowFloor.floorRoomInfo[nowRoom].spawnMonsters)
        {
            if (monsters.monsterObj.TryGetComponent<PoolableMono>(out PoolableMono obj))
            {
                obj.CustomInstantiate(monsters.monsterPos, obj.pair.enumtype);
            }
            else
            {
                Debug.LogWarning(monsters.monsterObj.name + $"({monsters.monsterObj.GetInstanceID()})" + "was not spawned");
            }

            Debug.Log($"i : {i + 1}, monsterpos : {monsters.monsterObj.transform.position}");
            //스폰 정보 없애기
            i++;
            if (i >= leftMonsters)
                break;

            //대충 여기서 웨이브보다 많이 스폰시 break
        }
    }

    //탈출구 랜덤스폰 
    void PortalSpawn()
    {
        bool spawned = false;
        if (!IsRandomExit)
        {
            foreach (var exit in floors[nowFloor].roomLists[nowRoom].exit)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                spawned = true;
            }
            return;
        }
        foreach (var exit in floors[nowFloor].roomLists[nowRoom].exit)
        {
            if (UnityEngine.Random.Range(0, 10) < 4)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                spawned = true;
            }
        }
        if (spawned == false)
            Portals[(int)floors[nowFloor].roomLists[nowRoom].exit[UnityEngine.Random.Range(0, floors[nowFloor].roomLists[nowRoom].exit.Count)]].SetActive(true);
    }

    public void OnPortalEnter(Transform obj,Transform player)
    {
        SetRoomMap();
        for(int i=0;i<Portals.Count;i++)
        {
            if(obj == Portals[i].transform)
            {
                int target = (i + 2) % Portals.Count;
                Vector3 dir = Vector3.zero;
                if (i % 2 == 1)
                    dir.x = i * -1 + 2;
                else if (i % 2 == 0)
                    dir.y = i - 1;

                player.position = Portals[target].transform.position + (dir * 2);

                return;
            }
        }
    }

    public void RoomTimerInit()
    {
        TimeManager.Instance.NowTime = floors[nowFloor].floorRoomInfo[nowRoom].timeLimit;
        TimeManager.Instance.StartTimer();
    }

    public void RoomEffectInit()
    {
        dirtEffect.Pause();
        roomStartTime = Time.time;
        var em = dirtEffect.emission;
        em.rateOverTime = 0;
        dirtEffect.Stop();
        dirtEffect.Play();
    }

    //층 마다 생성
    void StageGenerate()
    {
        MapInfoSO newMap = floors[0].CloneAndSetting();
        floors.Add(newMap);
    }

    void SetRoomMap()
    {
        foreach (var portal in Portals)
        {
            portal.SetActive(false);
        }

        Destroy(nowMap.gameObject);
        if (nowRoom != floors[nowFloor].floorRoomInfo.Count)
            nowMap = Instantiate(floors[nowFloor].floorRoomInfo[nowRoom].MapPrefab, transform.position, Quaternion.identity);
        AstarPath.active.Scan();
    }

    #region Flow Methods



    void RoomClear()
    {

    }

    void FloorClear()
    {
        //대충 다음층으로 넘어가지는 길 만들기
    }

    #endregion

}
