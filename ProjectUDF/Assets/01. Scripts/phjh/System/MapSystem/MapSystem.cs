using MapDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapSystem : MonoSingleton<MapSystem>
{
    //������ ������ �� ���� ������ ����ִ�?(�� ���� ����)
    //public List<MapInfoSO> StageInfo;

    //�� ���� ���۵Ǵ� �̺�Ʈ (���� ����)
    public event Action FloorStartEvent; //1��  -> ���� �����Ҷ� ����ȴ�?  ������ ������ ���⼭ �̷������?
    //public event Action FloorClearEvent; //2��  ->  �� ���� Ŭ���� ������ ����ȴ�? ���������� ���� ��Ż ������ ���⼭ �� �����̴�

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action RoomStartEvent;  //3��   ->  �� �濡 ����?����ȴ�?  �ð������� ���� ���Եȴ�
    public event Action RoomClearEvent;  //4��  ->   �� ���� Ŭ���� ������ ���´�.  ä�������� ���� ���Եȴ�

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
    public Action MonsterWaveClearEvent;  //5��  ->  ���̺꿡 ���?���͸� �� �������?����ȴ�?  ���� ���̺� ���� ��ȯ ���� �Ҷ� ���δ�

    public event Action MonsterKilledEvent; //6�� -> ���� ���������� ������ �̺�Ʈ, ���� ������ �������ָ� �ȴ�.

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

    public int ClearRoomCount { get; private set; } = 0;

	[Header("InGame Room's Values")]
    public float roomStartTime = 0;
    public int waveCount = -1;
    public int leftMonsters = 0;
    public bool IsRandomExit = false;

	private List<RoomInfoSO> CurFloorRoomList => floors[floorCount].floorRoomInfo; //���� ���� �� ���?
    private RoomInfoSO CurRoom => CurFloorRoomList[roomCount]; //���� ���� �� ���?�� ���õ� ��

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
    {
        floors[floorCount] = floors[floorCount].CloneAndSetting();      //���� Random���̸� ��
        dirtEffect.Play();
        WaveClear();
        SetNextRoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AstarPath.active.Scan();
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
		//TimeManager.Instance.RemainTime += CurRoom.RoomWaveData[waveCount].AddTime;
		waveCount++;
		if (waveCount == CurRoom.RoomWaveData.Count)
        {
            waveCount = 0;
            roomCount++;
            OnRoomClear();
            RoomClearEvent?.Invoke();
        }
        else if(waveCount < CurRoom.RoomWaveData.Count)
        {
			SpawnMonsters(2f);
		}
        else if(waveCount > CurRoom.RoomWaveData.Count)
        {
            Debug.LogWarning($"Out Of Index : WaveCount [{waveCount}]");
        }
    }

    public void OnRoomStart()
    {
        SetNextRoom();
        SpawnMonsters(2f);
	}

	private void OnRoomClear()
    {
        ClearRoomCount = ClearRoomCount + 1;
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

    void MapScan()
    {
        AstarPath.active.Scan();
    }

    void SpawnMonsters(float time)
    {
        TimeManager.Instance.StopTimer();
        Invoke(nameof(SpawnMonsters), time);
    }

    //���� ��ȯ�ϴ� �޼���
    private void SpawnMonsters()
    {
        TimeManager.Instance.StartTimer();
        if(CurRoom.RoomWaveData == null)
        {
			Debug.LogWarning($"{CurRoom.name}'s WaveData is Null");
			return;
		}

		if (CurRoom.RoomWaveData[waveCount] == null)
		{
			Debug.LogWarning($"{CurRoom.name}'s in WaveData[{waveCount}] is No Data");
			return;
		}

		List<MonsterInfo> SpawnList = CurRoom.RoomWaveData[waveCount].AppearMonsterInfo;
		leftMonsters = SpawnList.Count;

        for (int summonCount = 0; summonCount < leftMonsters; summonCount++)
        {
			if (SpawnList[summonCount].monsterObj == null)
			{
				Debug.LogWarning($"{CurRoom.name}'s in WaveData[{summonCount}] Object is Null");
			}

			if (SpawnList[summonCount].monsterPos == null)
			{
				Debug.LogWarning($"{CurRoom.name}'s in WaveData[{summonCount}] Position is Null");
			}

			if (SpawnList[summonCount].monsterObj.TryGetComponent(out EnemyMain obj))
            {
                if(obj.pair.enumtype == PoolObjectListEnum.None) continue;
                obj.CustomInstantiate(SpawnList[summonCount].monsterPos, obj.pair.enumtype);

				Debug.Log($"Summon Success : NAME[{SpawnList[summonCount].monsterObj.name}] POS[{SpawnList[summonCount].monsterObj.transform.position}]");
			}
            else
            {
                Debug.LogWarning(SpawnList[summonCount].monsterObj.name + $"({SpawnList[summonCount].monsterObj.GetInstanceID()})" + "was not isSpawnPortal");
            }
        }
	}

	//Ż�ⱸ �������� 
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
        TimeManager.Instance.StartTimer();
		TimeManager.Instance.RemainTime = CurRoom.timeLimit;
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

	//�� ���� ����
	private void StageGenerate()
    {
        FloorInfoSO newMap = floors[floorCount].CloneAndSetting(false);
        floors.Add(newMap);
    }

	private void SetNextRoom()
    {
        TimeManager.Instance.StopTimer();
        foreach (var portal in Portals)
        {
            portal.SetActive(false);
        }
        if (roomCount != CurFloorRoomList.Count)
        {
		    SetTileData(ObstacleTileMap, CurRoom.Obstacle);
		    SetTileData(DecorateTileMap, CurRoom.Decorate);
        }
        Invoke(nameof(MapScan), 0.1f);

        RoomTimerInit();
        RoomEffectInit();
	}
    
    private void SetTileData(Tilemap SetTilemap, PlacedTileData LoadData) //���� ���� ���� ���� ������ �ִ� ��ġ Ÿ�� �����͸� ȣ���� ��ġ��
    {
        if(LoadData == null)
        {
            Debug.LogError($"{floorCount}F{roomCount}R : {SetTilemap.name}'s Data is Null");
            return;
        }

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
        //���� ���������� �Ѿ���� �� �����?
    }

    #endregion

}
