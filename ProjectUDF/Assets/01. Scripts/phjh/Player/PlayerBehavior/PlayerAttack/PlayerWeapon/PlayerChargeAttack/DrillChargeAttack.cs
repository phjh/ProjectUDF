using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DrillChargeAttack : PlayerChargeAttack
{
    public override void OnAttackPrepare()
    {
        if (!CanAttack())
            return;

        _showRange = true;
        attackRange.SetActive(true);
        charged += Time.deltaTime;
    }

    protected override void OnAttackStart()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnAttackEnd()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnAttacking()
    {
        throw new System.NotImplementedException();
    }
}
