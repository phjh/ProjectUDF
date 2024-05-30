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
    protected string WhatIsEnemy = "Player";
    [SerializeField]
    protected string WhatIsObstacle = "Obstacle";

    public virtual void Shoot(Vector2 direction) { }
    public virtual void Shoot(Transform direction) { }

	private void OnEnable()
	{
		Debug.Assert(BulletEnum != PoolObjectListEnum.None, $"{gameObject.name}'s Bullet Enum is None [NullRef,PLZ Checking Inspector]");
	}

	protected void PushBullet()
    {
        StopAllCoroutines();
        PoolManager.Instance.Push(this, BulletEnum);
    }
}
