using System;
using UnityEngine;

public abstract class PlayerBaseAttack : PlayerWeaponAttack
{
    [SerializeField]
    protected float timeToAttacking; 
    [SerializeField]
    protected float timeToEnd;
    [SerializeField]
    protected Vector3 attackPos;

    protected bool _showRange;

    protected override void OnAttackStart()
    {
        Invoke(nameof(OnAttacking), timeToAttacking);
    }

    protected override void OnAttacking()
    {
        Invoke(nameof(OnAttackEnd), timeToEnd);
    }

}
