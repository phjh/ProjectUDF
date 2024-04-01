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

    //�� ���� ���۵Ǵ� �̺�Ʈ (���� ����)
    public event Action FloorStartEvent; //1��  -> ���� �����Ҷ� ����ȴ�.  ������ ������ ���⼭ �̷������.
    public event Action FloorClearEvent; //2��  ->  �� ���� Ŭ���� ������ ����ȴ�. ���������� ���� ��Ż ������ ���⼭ �� �����̴�

    //�� ���� ���۵Ǵ� �̺�Ʈ
    public event Action RoomStartEvent;  //3��   ->  �� �濡 ���� ����ȴ�.  �ð������� ���� ���Եȴ�
    public event Action RoomClearEvent;  //4��  ->   �� ���� Ŭ���� ������ ���´�.  ä�������� ���� ���Եȴ�

    //���� ���̺� �������� ���۵Ǵ� �̺�Ʈ
    public event Action MonsterWaveClearEvent;  //5��  ->  ���̺꿡 ��� ���͸� �� ������� ����ȴ�.  ���� ���̺� ���� ��ȯ ���� �Ҷ� ���δ�

    public event Action MonsterKilledEvent; //6�� -> ���� ���������� ������ �̺�Ʈ, ���� ������ �������ָ� �ȴ�.

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
