using System;
using System.Collections.Generic;

public class MapSystem : MonoSingleton<MapSystem>
{
    //보스를 제외한 한 층의 정보를 담고있다 (각 방의 정보)
    public List<MapInfoSO> StageInfo;

    //층 따라 시작되는 이벤트
    public event Action FloorStartEvent; //1번
    public event Action FloorClearEvent; //2번

    //방 마다 시작되는 이벤트
    public event Action MapStartEvent;  //3번
    public event Action MapClearEvent;  //4번

    //몬스터 웨이브 깰때마다 시작되는 이벤트
    public event Action MonsterWaveClearEvent;  //5번


    public void ActionInvoker(int thing)
    {
        switch (thing)
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
            default:
                break;
        }

    }

}
