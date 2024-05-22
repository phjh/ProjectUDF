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
    //������ ������ �� ���� ������ ����ִ� (�� ���� ����)
    //public List<MapInfoSO> StageInfo;

    //�� ���� ���۵Ǵ� �̺�Ʈ (���� ����)
    public event Action FloorStartEvent; //1��  -> ���� �����Ҷ� ����ȴ�.  ������ ������ ���⼭ �̷������.
    //public event Action FloorClearEvent; //2��  ->  �� ���� Ŭ���� ������ ����ȴ�. ���������� ���� ��Ż ������ ���⼭ �� �����̴�

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action RoomStartEvent;  //3��   ->  �� �濡 ���� ����ȴ�.  �ð������� ���� ���Եȴ�
    public event Action RoomClearEvent;  //4��  ->   �� ���� Ŭ���� ������ ���´�.  ä�������� ���� ���Եȴ�

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
    public Action MonsterWaveClearEvent;  //5��  ->  ���̺꿡 ��� ���͸� �� ������� ����ȴ�.  ���� ���̺� ���� ��ȯ ���� �Ҷ� ���δ�

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

    [SerializeField]
    private List<FloorInfoSO> floors;

    [SerializeField]
    private ParticleSystem dirtEffect;

    [SerializeField]
    private List<GameObject> Portals;

    private GameObject nowMap;

    public GameObject ExitPrefab;

    public int nowFloor = 0;
    public int roomCount = 0;
    public int nowWave = -1;
    public int leftMonsters = 0;
    public float roomStartTime = 0;
    public bool IsRandomExit = false;

    private void Start()
    {
        floors[nowFloor] = floors[nowFloor].CloneAndSetting();      //���� Random���̸� ��
        nowMap = Instantiate(floors[nowFloor].floorRoomInfo[roomCount].MapPrefab);
        dirtEffect.Play();
        WaveClear();
        SetNextRoom();
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
        if (Time.time - roomStartTime > floors[nowFloor].floorRoomInfo[roomCount].timeLimit)
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
        MapSystem.Instance.MonsterKilledEvent += OnMonsterDead;
    }

    private void OnDisable()
    {
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
        if (nowWave == floors[nowFloor].floorRoomInfo[roomCount].monsterWaves - 1)
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
        RoomTimerInit();
        RoomEffectInit();
        SetNextRoom();
        SetLeftMonsters();
        SpawnMonsters();
        RoomClearEvent?.Invoke();
	}

    void OnRoomClear()
    {
        if (roomCount == floors[nowFloor].floorRoomInfo.Count)
        {
            OnFloorClear();
            nowFloor++;
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

    void SetLeftMonsters() => leftMonsters = floors[nowFloor].floorRoomInfo[roomCount].numberOfMonsters[0];

    //���� ��ȯ�ϴ� �޼���
    void SpawnMonsters()
    {
        FloorInfoSO nowFloor = floors[this.nowFloor];
        leftMonsters = nowFloor.floorRoomInfo[roomCount].numberOfMonsters[nowWave];

        int i = 0;
        foreach (var monsters in nowFloor.floorRoomInfo[roomCount].spawnMonsters)
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
            //���� ���� ���ֱ�
            i++;
            if (i >= leftMonsters)
                break;

            //���� ���⼭ ���̺꺸�� ���� ������ break
        }
    }

    //Ż�ⱸ �������� 
    void PortalSpawn()
    {
        bool isSpawnPortal = false;
        if (!IsRandomExit)
        {
            foreach (var exit in floors[nowFloor].roomList[roomCount].exit)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                isSpawnPortal = true;
            }
            return;
        }
        foreach (var exit in floors[nowFloor].roomList[roomCount].exit)
        {
            if (UnityEngine.Random.Range(0, 10) < 4)
            {
                Portals[(int)exit].gameObject.SetActive(true);
                isSpawnPortal = true;
            }
        }
        if (isSpawnPortal == false)
            Portals[(int)floors[nowFloor].roomList[roomCount].exit[UnityEngine.Random.Range(0, floors[nowFloor].roomList[roomCount].exit.Count)]].SetActive(true);
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
		TimeManager.Instance.NowTime = floors[nowFloor].floorRoomInfo[roomCount].timeLimit;
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

    //�� ���� ����
    void StageGenerate()
    {
        FloorInfoSO newMap = floors[0].CloneAndSetting(false);
        floors.Add(newMap);
    }

    void SetNextRoom()
    {
        foreach (var portal in Portals)
        {
            portal.SetActive(false);
        }

        Destroy(nowMap.gameObject);
        if (roomCount != floors[nowFloor].floorRoomInfo.Count)
            nowMap = Instantiate(floors[nowFloor].floorRoomInfo[roomCount].MapPrefab, transform.position, Quaternion.identity);
        AstarPath.active.Scan();
    }

    #region Flow Methods

    void RoomClear()
    {

    }

    void FloorClear()
    {
        //���� ���������� �Ѿ���� �� �����
    }

    #endregion

}
