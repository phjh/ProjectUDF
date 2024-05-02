using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBaseAttack : PlayerBaseAttack, IStopAttractable
{

    protected override void Start()
    {
        base.Start();
    }

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        if (!stoneActived)
            AdditionalAttack[PlayerMain.Instance.EquipMainOre].Invoke();

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

    void OnForceStoneSelectedAction()
    {
        //��������~~


        //���콺 ������ �����ѽð�

        //���콺 ���� ������ �ð�

        //�⺻ ����� �޾�
        //���� ���� ����� ��ü���ְ�
        //������ ������ ������ �⺻����� ���� / �����ٲܶ� �ٽ� ���õǰ� �ٲٱ�
        //
    }

    protected override void StrengthStoneAttack()
    {
        attackRange.transform.localScale = attackRange.transform.localScale * 1.2f;
    }

    protected override void LuckyStoneAttack()
    {
        throw new NotImplementedException();
    }

    protected override void AttackSpeedStoneAttack()
    {
        throw new NotImplementedException();
    }

    protected override void MoveSpeedStoneAttack()
    {
        throw new NotImplementedException();
    }
}
