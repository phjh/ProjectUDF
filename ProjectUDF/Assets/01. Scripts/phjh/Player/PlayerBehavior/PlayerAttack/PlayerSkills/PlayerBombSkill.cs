using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombSkill : PlayerSkillAttack
{
    [SerializeField]
    private float bombtime = 3f;

    private float lastSkillusedTime;
    private bool skillused = false;

    private Coroutine coroutine;

    public Slider Slider;
    private Collider2D collider;

    private void Start()
    {
        collider = GetComponentInChildren<Collider2D>();
    }

    protected override void TryAttack()
    {
        if (coroutine != null)
            return;
        if (skillused && Time.time - lastSkillusedTime <= 3f)
        {
            StopCoroutine(coroutine);
            EndSkill(); 
        }
        else
        {
            base.TryAttack();
        }
        //do something
        coroutine = StartCoroutine(InvokeSkillWithWait());
    }

    IEnumerator InvokeSkillWithWait()
    {
        float time = 0;
        while(time < bombtime)
        {
            time += 0.01f;
            Slider.value = (time + 0.01f) / bombtime;
            yield return new WaitForSeconds(0.01f);
        }
        InvokeSkill();
        yield return new WaitForSeconds(0.3f);
        EndSkill();
    }

    private void InvokeSkill()
    {
        lastSkillusedTime = 0f;
        skillused = false;
        collider.enabled = true;

        //스킬 이펙트와 콜라이더 구현

    }

    private void EndSkill()
    {
        collider.enabled = false;
        coroutine = null;
        this.gameObject.SetActive(false);
    }

    Queue<GameObject> hitCooldownObjs = new();

    List<GameObject> hitList;

    private void OnEnable()
    {
        hitList = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy) && !hitList.Contains(collision.gameObject))
        {
            Debug.Log("Connected trigger damage : " + PlayerMain.Instance.recentDamage);
            hitList.Add(collision.gameObject);
            EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), CalculateDamage(damageFactor, true), 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            GameManager.Instance.ShakeCamera();
        }
        else if (collision.CompareTag("Enemy"))
        {
            Debug.Log("이거 왜뜸?");
        }
        else
        {
            Debug.Log($"collisoin : {collision}");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy) && !hitCooldownObjs.Contains(collision.gameObject))
        {
            hitCooldownObjs.Enqueue(collision.gameObject);
            EffectSystem.Instance.EffectsInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), PlayerMain.Instance.recentDamage, 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            GameManager.Instance.ShakeCamera();
        }
    }

}
