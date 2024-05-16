using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseAttack : PlayerWeaponAttack
{
    [SerializeField]
    protected float damageFactor;

    [SerializeField]
    protected GameObject attackRange;

    [SerializeField]
    protected Collider2D atkcollider;

    protected override void TryAttack()
    {
        if (!CanAttack())
            return;

        base.TryAttack();
        Debug.Log("attack Invoked");
        OnAttackStart();
    }

    protected override void OnAttackStart()
    {
        //기존 함수 실행
        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {
        //기존 함수 실행
        Invoke(nameof(OnAttackEnd), timeToAttacking);
    }



}
