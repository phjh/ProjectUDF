using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerStat stat = collision.transform.GetComponent<Player>()._playerStat;
            stat.EditPlayerHP(-1);
            MapSystem.Instance.ActionInvoker(MapEvents.MonsterKill);
            PoolManager.Instance.Push(this.gameObject.GetComponent<PoolableMono>().pair);
        }

    }
}
