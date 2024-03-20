using System;
using System.Collections.Generic;

public class MapSystem : MonoSingleton<MapSystem>
{
    //������ ������ �� ���� ������ ����ִ� (�� ���� ����)
    public List<MapInfoSO> StageInfo;

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action FloorStartEvent; //1��
    public event Action FloorClearEvent; //2��

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action MapStartEvent;  //3��
    public event Action MapClearEvent;  //4��

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
    public event Action MonsterWaveClearEvent;  //5��


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
