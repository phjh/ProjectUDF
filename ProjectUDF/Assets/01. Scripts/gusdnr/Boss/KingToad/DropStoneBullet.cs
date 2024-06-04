using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropStoneBullet : BulletMono
{
	[Range(0.5f, 5)][SerializeField] private float Radius;
	[Range(0, 3)][SerializeField] private float WaitTerm;
	[Range(1, 10)][SerializeField] private float ChaseSpeed;
	private bool canMove = true;
	private bool searchInColider = false;

	private CircleCollider2D AttackRange;
	private Transform Target;

	public override void ResetPoolingItem()
	{
		if(AttackRange == null) AttackRange = GetComponent<CircleCollider2D>();
		AttackRange.radius = Radius;
		
		Target = null;

		canMove = true;
		searchInColider = false;
	}

	private void Update()
	{
		if(canMove)
		{
			MoveProjctile();
		}
	}

	private void MoveProjctile()
	{
		Vector3 movedir = (Target.position - transform.localPosition).normalized;
		transform.localPosition += movedir * ChaseSpeed * Time.deltaTime;
	}

	public override void Shoot(Transform direction)
	{
		ResetPoolingItem();

		Target = direction;
		transform.localPosition = Target.position;

		StartCoroutine(SearchTimer());
	}

	private IEnumerator SearchTimer()
	{
		StartCoroutine(BulletLifeTimer());
		yield return new WaitForSeconds(WaitTerm);
		canMove = false;
		searchInColider = true;
	}

	private IEnumerator BulletLifeTimer()
	{
		yield return new WaitForSeconds(BulletLifeTime);
		PushBullet();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (searchInColider)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer(WhatIsEnemy))
			{
				Debug.Log("Hit Player");
				if (collision.TryGetComponent(out PlayerMain player)) player.GetDamage();
				PushBullet();
			}
		}
	}

}
