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
            Debug.Log("Connected trigger damage : " + _playerAtk.ResentDamage);
            EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, _playerAtk._player._playerStat.Strength.GetValue(), _playerAtk.ResentDamage, 0.5f, collision.transform.position);
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
