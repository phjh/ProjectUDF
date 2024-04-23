public abstract class PlayerWeaponAttack : PlayerBAttack
{
    protected float SetCooltime;
    protected float nowTime;

    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent.Invoke();
    }

    protected abstract void OnAttackStart();    //�����غ�/��¡ �� ���� ������ �����ش�
    protected abstract void OnAttacking();      //�������� ������ ó�����ش�
    protected abstract void OnAttackEnd();      //���� ���� ��ó���� ���ش�
}
