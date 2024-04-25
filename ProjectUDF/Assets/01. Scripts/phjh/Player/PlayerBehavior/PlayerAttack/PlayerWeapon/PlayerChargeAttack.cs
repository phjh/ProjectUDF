using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerChargeAttack : PlayerWeaponAttack
{
    [SerializeField]
    protected float maxChargeTime;
    [SerializeField]
    protected float maxChargedFactor;

}
