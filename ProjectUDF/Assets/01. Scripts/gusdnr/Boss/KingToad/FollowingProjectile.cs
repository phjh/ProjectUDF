using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingProjectile : BulletMono
{
	[Header("Projectile Values")]
	[Range(0, 15)] public float moveSpeed;
	[Range(0, 5)]public int repeatCount;
	[Range(0, 1)]public float activeTime;
	[Range(0, 5)]public float attackTerm;
	public Vector2[] attackRanges;

	private int attackCount = 0;
	private bool isActiveRange = false;

	private Transform Target;
	private Coroutine AttackCoroutine;
	private BoxCollider2D AttackRange;

	public override void ResetPoolingItem()
	{
		attackCount = 0;
		Target = null;
		AttackCoroutine = null;
		isActiveRange = false;
		if(AttackRange == null) GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
        if (Target != null)
        {
			MoveProjctile();
		}
	}

	private void MoveProjctile()
	{
		Vector3 movedir = (transform.localPosition - Target.position).normalized;
		transform.localPosition = movedir * moveSpeed * Time.fixedDeltaTime;
	}

	public override void Shoot(Transform direction)
	{
		Target = direction;

		AttackCoroutine = StartCoroutine(ProjectileCoroutine());
	}

	private IEnumerator ProjectileCoroutine()
	{
		for (int c= 0; c < repeatCount; c++)
		{
			yield return ActvieAttack();
			attackCount += 1;
			yield return new WaitForSeconds(attackTerm);
		}

		PushBullet();
	}

	private IEnumerator ActvieAttack()
	{
		if (attackRanges[attackCount] != null)
		{
			AttackRange.size = attackRanges[attackCount];
		}
		else
		{
			Debug.LogWarning($"{gameObject.name}'s AttackRange is Not Set. OutOfRange [attackRanges]");
		}
		isActiveRange = true;
		yield return new WaitForSeconds(activeTime);	
		isActiveRange = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isActiveRange)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer(WhatIsObstacle))
			{
				Debug.Log("Hit Obstacle");
				PushBullet();
			}
			if (collision.gameObject.layer == LayerMask.NameToLayer(WhatIsEnemy))
			{
				Debug.Log("Hit Player");
				if (collision.TryGetComponent(out PlayerMain player)) player.GetDamage();
			}
		}
	}
}
