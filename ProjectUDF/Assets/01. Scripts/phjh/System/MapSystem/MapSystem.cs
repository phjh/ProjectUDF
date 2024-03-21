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
    //������ ������ �� ���� ������ ����ִ� (�� ���� ����)
    //public List<MapInfoSO> StageInfo;

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action FloorStartEvent; //1��
    public event Action FloorClearEvent; //2��

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action MapStartEvent;  //3��
    public event Action MapClearEvent;  //4��

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
    public event Action MonsterWaveClearEvent;  //5��
    public event Action MonsterKilledEvent; //6��

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
