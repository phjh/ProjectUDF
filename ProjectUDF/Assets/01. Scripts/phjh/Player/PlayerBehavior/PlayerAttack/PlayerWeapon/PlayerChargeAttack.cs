using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerChargeAttack : PlayerWeaponAttack
{

    [SerializeField]
    protected GameObject attackRange;
    [SerializeField]
    protected Collider2D atkcollider;

    protected float charged = 0;

    [SerializeField]
    protected float maxChargeTime;
    [SerializeField]
    protected float maxChargedFactor;

}
