using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    [SerializeField] PlayerAttack _playerAtk;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            Debug.Log("@@@@@trigger damage : " + _playerAtk.ResentDamage);
            enemy.GetDamage(_playerAtk.ResentDamage);
        }
    }
    

}
