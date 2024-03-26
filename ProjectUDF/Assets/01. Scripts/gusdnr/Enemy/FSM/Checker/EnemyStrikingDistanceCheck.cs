using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
	public GameObject PlayerTarget { get; set; }
	private EnemyMain _enemy;
	private CircleCollider2D Sensor;

	private void Start()
	{
		PlayerTarget = GameManager.Instance.player.gameObject;
		if (_enemy == null) _enemy = GetComponentInParent<EnemyMain>();
		if (Sensor == null) Sensor = GetComponent<CircleCollider2D>();
		Sensor.radius = _enemy.StrikingRadius;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == PlayerTarget)
		{
			_enemy.SetStrikingDistance(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject == PlayerTarget)
		{
			_enemy.SetStrikingDistance(false);
		}
	}
}
