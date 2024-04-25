using UnityEngine;

public abstract class PlayerWeaponAttack : PlayerAttack
{
    [SerializeField]
    protected float timeToAttacking;
    [SerializeField]
    protected float timeToEnd;

    protected bool _showRange;

    protected virtual bool CanAttack()
    {
        if (PlayerMain.Instance.isAttacking)
        {
            Debug.Log("cooltime");
            return false;
        }
        return true;
    }
    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent?.Invoke();
    }

    public abstract void OnAttackPrepare();     //�����غ�/��¡ �� ���� ������ �����ش�
    protected abstract void OnAttackStart();    //�������� ������ ó�����ش�
    protected abstract void OnAttacking();      //���� ���� ��ó���� ���ش�
    protected abstract void OnAttackEnd();      //���� ��Ÿ�� ���� ������ �۾��� ���ش�

}
