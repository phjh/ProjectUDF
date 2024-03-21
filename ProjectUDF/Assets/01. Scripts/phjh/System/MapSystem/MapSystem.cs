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

    //층 따라 시작되는 이벤트
    public event Action FloorStartEvent; //1번
    public event Action FloorClearEvent; //2번

    //방 마다 시작되는 이벤트
    public event Action MapStartEvent;  //3번
    public event Action MapClearEvent;  //4번

    //몬스터 웨이브 깰때마다 시작되는 이벤트
    public event Action MonsterWaveClearEvent;  //5번
    public event Action MonsterKilledEvent; //6번

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
                MapStartEvent?.Invoke();
                break;
            case 4:
                MapClearEvent?.Invoke();
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
