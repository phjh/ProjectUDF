using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickaxeBaseAttack : PlayerBaseAttack, IStopAttractable
{
    [SerializeField]
    GameObject attackRange;
    [SerializeField]
    Collider2D atkcollider;

    protected override void TryAttack()
    {
        if(!CanAttack())
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }

    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        //공격 범위 표시
        _showRange = true;
        attackRange.gameObject.SetActive(true);
    }

    protected override void OnAttackStart()
    {
        //공격중
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage();
        Debug.Log("damage : " + damage);

        //공격범위 고정
        StopAiming();

        //이펙트 재생
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, attackRange.transform.position + Vector3.up / 2, 0.2f);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, attackRange.transform.position + Vector3.up / 3, 0.4f);

        //움직임 제한
        PlayerMain.Instance.canMove = false;
        
        atkcollider.enabled = true;

        //기존 함수 실행
        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {
        //공격 시작에서 공격 처리 관련된 것들 실행하기
        StartAiming();

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);

        //기존 함수 실행
        Invoke(nameof(OnAttackEnd), timeToEnd);
    }

    protected override void OnAttackEnd()
    {
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.isAttacking = false;
        PlayerMain.Instance.canAttack = true;
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
}
