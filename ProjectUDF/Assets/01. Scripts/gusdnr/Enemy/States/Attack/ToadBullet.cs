using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadBullet : BulletMono
{
	[Header("Movement Speed")]
    [Range(0, 10)] public float minSpeed;
    [Range(0, 10)] public float maxSpeed;

    private Rigidbody2D RockRB;
	private float curSpeed;

	private void Awake()
	{
		RockRB = GetComponent<Rigidbody2D>();
	}

	public override void ResetPoolingItem()
	{
		if(RockRB == null) RockRB = GetComponent<Rigidbody2D>();
	}

	public override void Shoot(Vector2 direction)
	{
		StartCoroutine(BulletSpeedControl(direction));
		Invoke(nameof(PushBullet), BulletLifeTime);
	}

	private IEnumerator BulletSpeedControl(Vector2 direction)
	{
		float time = 0;
		curSpeed = maxSpeed;
		while(curSpeed >= minSpeed)
		{
			curSpeed = Mathf.Clamp(Mathf.Lerp(curSpeed, minSpeed, time), minSpeed, maxSpeed);
			time += Time.deltaTime;
			RockRB.velocity = direction * curSpeed;
			yield return null;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == WhatIsObstacle)
		{
			PushBullet();
		}
		if(collision.gameObject.layer == WhatIsEnemy)
		{
			if(collision.TryGetComponent(out PlayerMain player)) player.GetDamage();
			PushBullet();
		}
	}
}
