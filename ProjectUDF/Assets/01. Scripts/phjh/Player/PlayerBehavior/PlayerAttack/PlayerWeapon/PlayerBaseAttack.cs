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
        //���� �Լ� ����
        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {
        //���� �Լ� ����
        Invoke(nameof(OnAttackEnd), timeToAttacking);
    }



}
