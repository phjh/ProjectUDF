using System;
using System.Collections.Generic;

public class MapSystem : MonoSingleton<MapSystem>
{
    //보스를 제외한 한 층의 정보를 담고있다 (각 방의 정보)
    public List<MapInfoSO> StageInfo;

    //층 따라 시작되는 이벤트
    public event Action StageStartEvent;
    public event Action StageClearEvent;

    //방 마다 시작되는 이벤트
    public event Action MapStartEvent;
    public event Action MapClearEvent;

    //몬스터 웨이브 깰때마다 시작되는 이벤트
    public event Action MonsterWaveClearEvent;

    public int leftRooms = 0;
    public int leftMonsters = 0;

    public void StageFlow()
    {
        for(int i=0;i<StageInfo.Count;i++)
        {
            //MapStartEvent?.Invoke();
            //MonsterWaveClearEvent?.Invoke();
            //MapClearEvent?.Invoke();

        }
        //여기서 보스방으로 가는 로직 짜기


    }



}
