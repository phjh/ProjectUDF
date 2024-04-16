using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            return;
        }
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.GetDamage();
        }

    }

}
