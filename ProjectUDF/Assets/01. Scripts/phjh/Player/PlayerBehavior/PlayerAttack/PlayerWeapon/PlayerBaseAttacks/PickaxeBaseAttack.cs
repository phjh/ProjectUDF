using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBaseAttack : PlayerBaseAttack, IStopAttractable
{
    float baseFactor;

    protected override void Start()
    {
        base.Start();
        baseFactor = damageFactor;
    }

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        InvokeStoneAttack();

        stoneActived = true;

        //���� ���� ǥ��
        Debug.Log("prepare");
        _showRange = true;
        attackRange.gameObject.SetActive(true);
    }

    protected override void OnAttackStart()
    {
        if (!_showRange)
        {
            Debug.LogWarning("Range is not enabled");
            return;
        }

        //������
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //������ ���ϱ�
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        //���ݹ��� ����
        StopAiming();

        //����Ʈ ���
        EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //������ ����
        PlayerMain.Instance.canMove = false;
        
        atkcollider.enabled = true;

        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {
        //���� ���ۿ��� ���� ó�� ���õ� �͵� �����ϱ�
        StartAiming();

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);

        base.OnAttacking();
    }

    protected override void OnAttackEnd()
    {
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
        stoneActived = false;
        Debug.Log("onattackend");
    }


    public void StartAiming()
    {
        PlayerMain.Instance.playerAim.enabled = true; 
    }

    public void StopAiming()
    {
        PlayerMain.Instance.playerAim.enabled = false;
    }

    [SerializeField]
    [Tooltip("���ǵ� ���� ���� ���� ���")]
    private float StrengthStoneMultiply;
    protected override void StrengthStoneAttack()
    {
        if (!isActiveonce)
        {
            attackRange.transform.localScale = attackRange.transform.localScale * StrengthStoneMultiply;
            isActiveonce = true;
        }
        else
        {
            attackRange.transform.localScale = attackRange.transform.localScale / StrengthStoneMultiply;
            isActiveonce = false;
        }
    }

    protected override void LuckyStoneAttack()
    {
        Debug.Log("asdf");
    }


    [SerializeField]
    [Tooltip("���ӵ� ���� ũ���� ���")]
    private float AttackSpeedStoneFactor;
    [SerializeField]
    [Tooltip("���ӵ� �޺� �ʱ�ȭ �ð�")]
    private float attackSpeedStoneComboResetTime;
    int nowattack;
    float beforeTime;
    protected override void AttackSpeedStoneAttack()
    {
        damageFactor = baseFactor;
        if(beforeTime + attackSpeedStoneComboResetTime < Time.time) //�޺�����
        {
            nowattack = 0;
            beforeTime = Time.time;
            nowattack++;
        }
        else
        {
            if(nowattack == 2)
            {
                damageFactor += AttackSpeedStoneFactor;
                nowattack = 0;
            }
            else
            {
                nowattack++;
                beforeTime = Time.time;
            }
        }

    }

    protected override void MoveSpeedStoneAttack()
    {
        Debug.Log("��Ŭ�� �̼� ȿ���� �����!");
    }
}
