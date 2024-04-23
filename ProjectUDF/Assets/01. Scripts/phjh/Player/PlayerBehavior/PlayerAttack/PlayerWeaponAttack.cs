public abstract class PlayerWeaponAttack : PlayerBAttack
{
    protected float SetCooltime;
    protected float nowTime;

    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent?.Invoke();
    }

    public abstract void OnAttackPrepare();     //�����غ�/��¡ �� ���� ������ �����ش�
    protected abstract void OnAttackStart();    //�������� ������ ó�����ش�
    protected abstract void OnAttacking();      //���� ���� ��ó���� ���ش�
    protected abstract void OnAttackEnd();      //���� ��Ÿ�� ���� ������ �۾��� ���ش�
}
