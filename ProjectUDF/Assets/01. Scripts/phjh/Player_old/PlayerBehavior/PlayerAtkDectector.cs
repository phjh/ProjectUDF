using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    Queue<GameObject> hitCooldownObjs = new();

    [SerializeField]
    private float hitCooldown;

    List<GameObject> hitList;

    [SerializeField]
    bool isStay = false;

    private void OnEnable()
    {
        hitList = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStay)
            return;

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy) && !hitList.Contains(collision.gameObject))
        {
            Debug.Log("Connected trigger damage : " + PlayerMain.Instance.recentDamage);
            hitList.Add(collision.gameObject);
            EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), PlayerMain.Instance.recentDamage, 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            GameManager.Instance.ShakeCamera();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isStay)
            return;

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy) && !hitCooldownObjs.Contains(collision.gameObject))
        {
            hitCooldownObjs.Enqueue(collision.gameObject);
            EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), PlayerMain.Instance.recentDamage, 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            GameManager.Instance.ShakeCamera();
            Invoke(nameof(SetCooldownObjList), hitCooldown);
        }
    }

    void SetCooldownObjList() => hitCooldownObjs.Dequeue();

}
