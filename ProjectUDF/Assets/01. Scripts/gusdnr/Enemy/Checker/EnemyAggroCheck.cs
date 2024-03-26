using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private EnemyMain _enemy;

	private void OnEnable()
	{
		PlayerTarget = GameManager.Instance.player.gameObject;

		if(_enemy == null) _enemy = GetComponent<EnemyMain>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject == PlayerTarget)
		{
			_enemy.SetAggroStatus(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject == PlayerTarget)
		{
			_enemy.SetAggroStatus(false);
		}
	}
}
