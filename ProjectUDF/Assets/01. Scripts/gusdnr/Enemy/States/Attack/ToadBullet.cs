using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadBullet : BulletMono
{
	[Header("Movement Speed")]
	public float defaultTime;
    [Range(0, 15)] public float maxSpeed;
	[Range(0, 15)] public float minSpeed;

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
		StartCoroutine(BulletSpeedControl(direction, defaultTime));
		StartCoroutine(RotateBullet());
		Invoke(nameof(PushBullet), BulletLifeTime);
	}

	private IEnumerator BulletSpeedControl(Vector2 direction, float defaultTime)
	{
		float time = 0;
		curSpeed = maxSpeed;
		RockRB.velocity = direction * curSpeed;
		yield return new WaitForSeconds(defaultTime);
		while(curSpeed >= minSpeed)
		{
			curSpeed = Mathf.Clamp(Mathf.Lerp(curSpeed, minSpeed, time), minSpeed, maxSpeed);
			time += Time.deltaTime;
			RockRB.velocity = direction * curSpeed;
			yield return null;
		}
	}

	private IEnumerator RotateBullet()
	{
		float roatationZ = 0;
		while (true)
		{
			roatationZ += 0.05f;
			transform.Rotate(0, 0, roatationZ, Space.Self);
			yield return null;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer(WhatIsObstacle))
		{
			Debug.Log("Hit Obstacle");
			PushBullet();
		}
		if(collision.gameObject.layer == LayerMask.NameToLayer(WhatIsEnemy))
		{
			Debug.Log("Hit Player");
			if(collision.TryGetComponent(out PlayerMain player)) player.GetDamage();
			PushBullet();
		}
	}
}
