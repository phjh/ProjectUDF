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

        //공격 범위 표시
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

        //공격중
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage(damageFactor);
        Debug.Log("damage : " + damage);

        //공격범위 고정
        StopAiming();

        //이펙트 재생
        EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //움직임 제한
        PlayerMain.Instance.canMove = false;
        
        atkcollider.enabled = true;

        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {
        //공격 시작에서 공격 처리 관련된 것들 실행하기
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
        //범위증가~~


        //마우스 누르기 시작한시간

        //마우스 떼서 공격한 시간

        //기본 계수를 받아
        //뎀지 식의 계수를 대체해주고
        //공격이 끝나면 원래의 기본계수로 변경 / 광석바꿀때 다시 세팅되게 바꾸기
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
