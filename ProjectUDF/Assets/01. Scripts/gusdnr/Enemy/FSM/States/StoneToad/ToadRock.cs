using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadRock : PoolableMono
{
    public float speed;
	public float liveTime;
    public LayerMask WhatIsEnemy;
    public LayerMask WhatIsObstacle;

    private Rigidbody2D RockRB;

	public override void ResetPooingItem()
	{
		if(RockRB == null) RockRB = GetComponent<Rigidbody2D>();
	}

	public void ShootingRock(Vector2 dir)
	{
		RockRB.velocity = dir * speed;
		StartCoroutine(LiveTimer());
	}

	private IEnumerator LiveTimer()
	{
		yield return new WaitForSeconds(liveTime);
		Debug.Log($"End Live Time : {gameObject.name}");
		PoolManager.Instance.Push(this);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.callbackLayers == WhatIsObstacle)
		{
			Debug.Log("Collision Obstacle");
			PoolManager.Instance.Push(this);
		}
		if(collision.callbackLayers == WhatIsEnemy)
		{
			GameManager.Instance.player._playerStat.EditPlayerHP(-1);
			PoolManager.Instance.Push(this);
		}
	}
}
