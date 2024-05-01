using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletMono : PoolableMono
{
    [Header("Set Bullet Pooling Type")]
    [SerializeField]
    public PoolObjectListEnum BulletEnum;
    [Header("Set Bullet Value")]
    [SerializeField]
    protected float BulletLifeTime;
    [SerializeField]
    protected LayerMask WhatIsEnemy;
    [SerializeField]
    protected LayerMask WhatIsObstacle;

    public virtual void Shoot(Vector2 direction) { }
    protected void PushBullet() => PoolManager.Instance.Push(this, BulletEnum);
}
