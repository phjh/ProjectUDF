using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : Player
{
    [SerializeField] Player _player;
    [SerializeField] PlayerAttackRange _range;

    #region 차징공격 관련

    [SerializeField] GameObject _rightattackRange;

    CircleCollider2D _rightAtkcol;

    #endregion

    public float ChargeTime = 1.6f;
    public float stiffenTime = 0.4f; //경직시간

    public float ResentDamage;

    protected void Start()
    {
        _playerStat = _player._playerStat;
        ResentDamage = _playerStat.Strength.GetValue();
        _rightAtkcol = _rightattackRange.GetComponent<CircleCollider2D>();
    }

    public IEnumerator NormalAttack()
    {
        float damage = CalculateDamage(0.8f);
        Debug.Log("damage : " + damage);
        yield return new WaitForSeconds(1.6f/ 2 / (_playerStat.AttackSpeed.GetValue()+1));
        _player.IsAttacking = false;
    }

    public IEnumerator ChargingAttack()
    {
        float pressTime = 0;
        float factor = 0;

        while (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            pressTime += 0.05f;
            float scale = Mathf.Lerp(0, ChargeTime, pressTime) / 2 + 1;
            _rightattackRange.transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.05f);
        }

        pressTime = Mathf.Clamp(pressTime, 0, ChargeTime);
        factor = Mathf.Lerp(-0.2f, 0.2f, pressTime / ChargeTime) + 1;
        Debug.Log($"time : {pressTime},  factor : {factor}");

        float damage = CalculateDamage(factor);
        Debug.Log("damage : " + damage);

        _rightAtkcol.enabled = true;
        _player.GetComponentInParent<PlayerMovement>().StopImmediately();
        _player.ActiveMove = false;
        PlayerAim aim = GetComponent<PlayerAim>();
        aim.enabled = false;
        EffectPoolableMono poolItem = PoolManager.Instance.Pop(EffectPoolingType.ChargeAttackEffect);
        poolItem.transform.position = _rightattackRange.transform.position;
        poolItem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.4f);
        _rightattackRange.gameObject.SetActive(false);
        _rightAtkcol.enabled = false;
        _player.ActiveMove = true;
        aim.enabled = true;
        yield return new WaitForSeconds(0.4f);
        PoolManager.Instance.Push(poolItem);
        yield return new WaitForSeconds(5f / 2 / (_playerStat.AttackSpeed.GetValue()+1));
        _player.IsAttacking = false;
    }

    [Obsolete]
    private void Update()
    {
        if (_player._isdodgeing)
        {
            StopAllCoroutines();
            _range.gameObject.SetActive(false);
            _rightattackRange.SetActive(false);
            _player.IsAttacking = false;
            return;
        }

        if (Input.GetMouseButton(0) && !_player.IsAttacking)
        {
            _player.IsAttacking = true;
            _range.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0) && _range.gameObject.active == true)
        {
            StartCoroutine(NormalAttack());
            _range.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1) && !_player.IsAttacking)
        {
            _player.IsAttacking = true;
            StartCoroutine(ChargingAttack());
            _rightattackRange.gameObject.SetActive(true);
        }

    }

    public float CalculateDamage(float factor)
    {
        float damage = 0;
        if (UnityEngine.Random.Range(0, 100) < GameManager.Instance.Lucky)
        {
            damage = ResentDamage * 1.3f * factor;
            Debug.Log("!!!!!!damage : " + damage);
        }
        else
        {
            damage = GameManager.Instance.Strength * factor;
        }
        ResentDamage = Mathf.Ceil(damage * 10) / 10;
        return damage;
    }
}
