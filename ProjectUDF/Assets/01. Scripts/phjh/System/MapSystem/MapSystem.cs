using System;

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
    public event Action FloorClearEvent; //2번  ->  각 층을 클리어 했을때 실행된다. 다음층으로 가는 포탈 생성을 여기서 할 예정이다

    //방 마다 시작되는 이벤트
    public event Action RoomStartEvent;  //3번   ->  각 방에 들어갈때 실행된다.  시간제한이 여기 포함된다
    public event Action RoomClearEvent;  //4번  ->   각 방을 클리어 했을때 나온다.  채광같은게 여기 포함된다

    //몬스터 웨이브 깰때마다 시작되는 이벤트
    public event Action MonsterWaveClearEvent;  //5번  ->  웨이브에 모든 몬스터를 다 잡았을시 실행된다.  다음 웨이브 몬스터 소환 등을 할때 쓰인다

    public event Action MonsterKilledEvent; //6번 -> 몹이 죽을때마다 실행할 이벤트, 몹이 죽을때 연결해주면 된다.

    public void ActionInvoker(MapEvents e)
    {
        switch ((int)e)
        {
            case 1:
                FloorStartEvent?.Invoke();
                break;
            case 2:
                FloorClearEvent?.Invoke(); 
                break;
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

}
