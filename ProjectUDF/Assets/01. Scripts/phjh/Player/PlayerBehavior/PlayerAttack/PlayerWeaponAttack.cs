using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeaponAttack : PlayerAttack
{
    public List<Action> AdditionalAttack = new();
    

    [SerializeField]
    protected float timeToAttacking;
    [SerializeField]
    protected float timeToEnd;

    // �h�籤�� ����, Dcttionary / ����Ʈ �ۼ�
    protected bool stoneActived;
    public bool isActiveonce;

    protected bool _showRange;

    protected virtual void Start()
    {
        Init();
    }

    protected void InvokeStoneAttack()
    {
        if (isActiveonce)
            return;
        if (stoneActived)
            return;
        AdditionalAttack[PlayerMain.Instance.nowStone].Invoke();
    }

    protected void Init()
    {
        AdditionalAttack.Add(() => { });
        AdditionalAttack.Add(StrengthStoneAttack);
        AdditionalAttack.Add(LuckyStoneAttack);
        AdditionalAttack.Add(AttackSpeedStoneAttack);
        AdditionalAttack.Add(MoveSpeedStoneAttack);
    }

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

    public virtual void OnAttackPrepare() { }     //�����غ�/��¡ �� ���� ������ �����ش�
    protected abstract void OnAttackStart();    //�������� ������ ó�����ش�
    protected abstract void OnAttacking();      //���� ���� ��ó���� ���ش�
    protected abstract void OnAttackEnd();      //���� ��Ÿ�� ���� ������ �۾��� ���ش�


    protected abstract void StrengthStoneAttack();
    protected abstract void LuckyStoneAttack();
    protected abstract void AttackSpeedStoneAttack();
    protected abstract void MoveSpeedStoneAttack();

}
