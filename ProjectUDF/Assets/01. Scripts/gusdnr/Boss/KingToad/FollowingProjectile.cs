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
	private Coroutine ProjectileCoroutine;

	public override void ResetPoolingItem()
	{
		Target = null;

		attackCount = 0;
		isActiveRange = false;
	}

	private void Update()
	{
        if (Target != null)
        {
			MoveProjctile();
		}
	}

	private void MoveProjctile()
	{
		Vector3 movedir = (Target.position - transform.localPosition).normalized;
		transform.localPosition += movedir * moveSpeed * Time.deltaTime;
	}
	public override void Shoot(Transform direction)
	{
		if (gameObject.activeInHierarchy == false)
		{
			gameObject.SetActive(true);
		}
		Target = direction;
		ProjectileCoroutine = StartCoroutine(ChangeScale());
		Invoke(nameof(PushBullet), BulletLifeTime);
	}

	private IEnumerator ChangeScale()
	{
		for (int c= 0; c < repeatCount; c++)
		{
			if (attackRanges[attackCount] != null)
			{
				transform.localScale = attackRanges[attackCount];
			}
			else
			{
				Debug.LogWarning($"{gameObject.name}'s AttackRange is Not Set. OutOfRange [attackRanges]");
			}
			isActiveRange = true;
			yield return new WaitForSeconds(activeTime);
			isActiveRange = false;
			attackCount += 1;
			yield return new WaitForSeconds(attackTerm);
		}

		PushBullet();
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
