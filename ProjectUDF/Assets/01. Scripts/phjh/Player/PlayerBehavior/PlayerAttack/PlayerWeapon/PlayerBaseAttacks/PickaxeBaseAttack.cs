using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PickaxeBaseAttack : PlayerBaseAttack, IAttractable
{
    [SerializeField]
    GameObject attackRange;
    [SerializeField]
    Collider2D collider;

    protected override void TryAttack()
    {
        Debug.Log("attack Invoked");
        OnAttackStart();
    }


    protected override void OnAttackStart()
    {
        //데미지 구하기
        float damage = CalculateDamage();
        Debug.Log("damage : " + damage);

        //공격범위 고정
        StopAiming();

        attackRange.gameObject.SetActive(true);

        //이펙트 재생
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, attackPos + Vector3.up / 2, 0.2f);
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, attackPos + Vector3.up / 3, 0.4f);

        //움직임 제한
        PlayerMain.Instance.canMove = false;

        //기존 함수 실행
        base.OnAttackStart();
    }

    protected override void OnAttacking()
    {
        //공격 시작에서 공격 처리 관련된 것들 실행하기
        StartAiming();

        PlayerMain.Instance.canMove = true;
        collider.enabled = false;
        attackRange.gameObject.SetActive(false);

        //기존 함수 실행
        base.OnAttacking();
    }

    protected override void OnAttackEnd()
    {
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
    }

    private IEnumerator NormalAttack()
    {
        Debug.Log("normal Attack!");
        yield return new WaitForSeconds(1f);
        Debug.Log("normal Attack Down!");
        //float damage = CalculateDamage(0.8f);
        //Debug.Log("damage : " + damage);
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, _baseAttackRange.transform.position + Vector3.up / 3, 0.4f);
        //EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, _baseAttackRange.transform.position + Vector3.up / 2, 0.2f);
        //_aim.enabled = false;
        //PlayerMain.Instance.canMove = false;
        //_baseAttackCol.enabled = true;
        //yield return new WaitForSeconds(0.3f);
        //_aim.enabled = true;
        //PlayerMain.Instance.canMove = true;
        //_baseAttackCol.enabled = false;
        //_baseAttackRange.gameObject.SetActive(false);
        //yield return new WaitForSeconds(1.5f / (2 + (PlayerMain.Instance.stat.AttackSpeed.GetValue() + 1)));
        //PlayerMain.Instance.canAttack = true;
        //PlayerMain.Instance.isAttacking = false;
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
