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

    [SerializeField]
    [Tooltip("힘의돌 쓸때 범위 증가 계수")]
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
    [Tooltip("공속돌 쓸때 크라켄 계수")]
    private float AttackSpeedStoneFactor;
    [SerializeField]
    [Tooltip("공속돌 콤보 초기화 시간")]
    private float attackSpeedStoneComboResetTime;
    int nowattack;
    float beforeTime;
    protected override void AttackSpeedStoneAttack()
    {
        damageFactor = baseFactor;
        if(beforeTime + attackSpeedStoneComboResetTime < Time.time) //콤보깨짐
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
        Debug.Log("좌클엔 이속 효과가 없어요!");
    }
}
