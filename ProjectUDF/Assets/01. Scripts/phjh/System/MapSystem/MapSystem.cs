using System;
using System.Collections.Generic;

public class MapSystem : MonoSingleton<MapSystem>
{
    //������ ������ �� ���� ������ ����ִ� (�� ���� ����)
    public List<MapInfoSO> StageInfo;

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action StageStartEvent;
    public event Action StageClearEvent;

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action MapStartEvent;
    public event Action MapClearEvent;

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
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
        //���⼭ ���������� ���� ���� ¥��


    }



}
