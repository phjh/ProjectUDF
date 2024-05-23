using MapDefine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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

    //public void ActionInvoker(MapEvents e)
    //{
    //    switch ((int)e)
    //    {
    //        case 1:
    //            FloorStartEvent?.Invoke();
    //            break;
    //        //case 2:
    //        //    FloorClearEvent?.Invoke();
    //            //break;
    //        case 3:
    //            RoomStartEvent?.Invoke();
    //            break;
    //        case 4:
    //            RoomClearEvent?.Invoke();
    //            break;
    //        case 5:
    //            MonsterWaveClearEvent?.Invoke();
    //            break;
    //        case 6:
    //            MonsterKilledEvent?.Invoke();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    [Header("MapSystem Datas")]
    [SerializeField]
    private List<FloorInfoSO> floors;

    [Header("Map System's Objects")]
    [SerializeField] private List<GameObject> Portals;
    [SerializeField] private GameObject ExitPrefab;
    [SerializeField] private Tilemap ObstacleTileMap;
    [SerializeField] private Tilemap DecorateTileMap;
	[SerializeField] private ParticleSystem dirtEffect;

	[Header("Map System's Values")]
    public int floorCount = 0;
    public int roomCount = 0;

	[Header("InGame Room's Values")]
    public float roomStartTime = 0;
    public int nowWave = -1;
    public int leftMonsters = 0;
    public bool IsRandomExit = false;

    private List<RoomInfoSO> CurFloorRoomList => floors[floorCount].floorRoomInfo; //현재 층의 방 목록
    private RoomInfoSO CurRoom => CurFloorRoomList[roomCount]; //현재 층의 방 목록 중 선택된 방

	private void OnEnable()
	{
		MapSystem.Instance.MonsterKilledEvent += OnMonsterDead;
	}

	private void OnDisable()
	{
		MapSystem.Instance.MonsterKilledEvent -= OnMonsterDead;
	}

	private void Start()
    {
        floors[floorCount] = floors[floorCount].CloneAndSetting();      //여기 Random붙이면 됨
        dirtEffect.Play();
        WaveClear();
        SetNextRoom();
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
        if (Time.time - roomStartTime > CurRoom.timeLimit)
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

    #region GameFlow

    public void OnMonsterDead()
    {
        leftMonsters--;
        if (leftMonsters <= 0)
        {
            WaveClear();
        }
    }

    private void WaveClear()
    {
        if (nowWave == CurRoom.monsterWaves - 1)
        {
            nowWave = 0;
            roomCount++;
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
        SetNextRoom();
        SetLeftMonsters();
        SpawnMonsters();
        RoomClearEvent?.Invoke();
	}

	private void OnRoomClear()
    {
        if (roomCount == CurFloorRoomList.Count)
        {
            OnFloorClear();
            floorCount++;
            roomCount = 0;
        }
        else
        {
            PortalSpawn();
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

	private void SetLeftMonsters() => leftMonsters = CurRoom.numberOfMonsters[0];

    //몬스터 소환하는 메서드
    private void SpawnMonsters()
    {
        leftMonsters = CurRoom.numberOfMonsters[nowWave];

        int i = 0;
        foreach (var monsters in CurRoom.spawnMonsters)
        {
            if (monsters.monsterObj.TryGetComponent<PoolableMono>(out PoolableMono obj))
            {
                obj.CustomInstantiate(monsters.monsterPos, obj.pair.enumtype);
            }
            else
            {
                Debug.LogWarning(monsters.monsterObj.name + $"({monsters.monsterObj.GetInstanceID()})" + "was not isSpawnPortal");
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
	private void PortalSpawn()
    {
        bool isSpawnPortal = false;
        if (!IsRandomExit)
        {
            foreach (var exit in CurRoom.exits)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                isSpawnPortal = true;
            }
            return;
        }       
        foreach (var exit in CurRoom.exits)
        {
            if (UnityEngine.Random.Range(0, 10) < 4)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                isSpawnPortal = true;
            }
        }
        if (isSpawnPortal == false)
            Portals[(int)CurRoom.exits[UnityEngine.Random.Range(0, CurRoom.exits.Count)]].SetActive(true);
    }

    public void OnPortalEnter(Transform obj,Transform player)
    {
        SetNextRoom();
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
        TimeManager.Instance.StopTimer();
		TimeManager.Instance.NowTime = CurRoom.timeLimit;
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
	private void StageGenerate()
    {
        FloorInfoSO newMap = floors[floorCount].CloneAndSetting(false);
        floors.Add(newMap);
    }

	private void SetNextRoom()
    {
        foreach (var portal in Portals)
        {
            portal.SetActive(false);
        }
        if (roomCount != CurFloorRoomList.Count)
        {
		    SetTileData(ObstacleTileMap, CurRoom.Obstacle);
		    SetTileData(DecorateTileMap, CurRoom.Decorate);
        }
        AstarPath.active.Scan();
        
		RoomTimerInit();
		RoomEffectInit();
	}

    private void SetTileData(Tilemap SetTilemap, PlacedTileData LoadData) //현재 층의 현재 방이 가지고 있는 배치 타일 데이터를 호출해 배치함
    {
        SetTilemap.ClearAllTiles();
        for(int count = 0; count < LoadData.PlacedPoses.Count; count++)
        {
            SetTilemap.SetTile(LoadData.PlacedPoses[count], LoadData.PlacedTiles[count]);
        }
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
