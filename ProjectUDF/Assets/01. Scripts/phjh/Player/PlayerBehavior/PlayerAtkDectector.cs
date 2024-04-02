using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    [SerializeField] PlayerAttack _playerAtk;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            Debug.Log("@@@@@trigger damage : " + _playerAtk.ResentDamage);
            EffectSystem.Instance.EffectInvoker(EffectPoolingType.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            enemy.Damage(_playerAtk.ResentDamage);
        }
        else if(collision.CompareTag("Enemy"))
        {
            Debug.Log("¿Ã∞≈ ø÷∂‰?");
        }
        else
        {
            Debug.Log($"collisoin : {collision}");
        }
    }
    

}
