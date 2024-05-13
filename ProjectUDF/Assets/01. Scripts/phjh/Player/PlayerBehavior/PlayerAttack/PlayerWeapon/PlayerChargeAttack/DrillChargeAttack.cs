using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DrillChargeAttack : PlayerChargeAttack
{
    

    public override void OnAttackPrepare()
    {
        Debug.Log("preparing");
        if (!CanAttack())
            return;

        _showRange = true;
        attackRange.SetActive(true);
        charged += Time.deltaTime;
    }

    protected override void OnAttackStart()
    {
        Debug.Log("atkStart");
        if (!_showRange)
            return;

        InvokeStoneAttack();

        float baseFactor = _damageFactor;
        _damageFactor = GetChargedFactor(charged);

        //공격중
        PlayerMain.Instance.isAttacking = true;
        PlayerMain.Instance.canAttack = false;

        //데미지 구하기
        float damage = CalculateDamage(_damageFactor);
        Debug.Log("damage : " + damage);

        atkcollider.enabled = true;

        _damageFactor = baseFactor;

        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {

        Debug.Log("onattacking");
        PlayerMain.Instance.canMove = true;
        atkcollider.enabled = false;
        attackRange.gameObject.SetActive(false);

        Invoke(nameof(OnAttackEnd), timeToEnd);
    }

    protected override void OnAttackEnd()
    {
        charged = 0f;
        attackRange.gameObject.SetActive(false);
        PlayerMain.Instance.canAttack = true;
        PlayerMain.Instance.isAttacking = false;
        Debug.Log("chargeAttackend");
    }

    protected override void StrengthStoneAttack()
    {
        int step = HowlongCharged(charged) + 1;

    }

    protected override void LuckyStoneAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackSpeedStoneAttack()
    {
        Debug.Log("공속 차징엔 읎다");
    }

    protected override void MoveSpeedStoneAttack()
    {
        throw new System.NotImplementedException();
    }
}
