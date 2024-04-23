using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : Player
{
    [SerializeField] public Player _player;
    [SerializeField] PlayerAttackRange _range;

    PolygonCollider2D leftAtkCol;

    [SerializeField] GameObject _rightattackRange;

    CircleCollider2D _rightAtkcol;

    Coroutine stopCoroutine;

    PlayerAim aim;

    public bool isCritical;

    public float ChargeTime = 1.6f;
    public float stiffenTime = 0.4f; //경직?�간

    public float ResentDamage;

    protected void Start()
    {
        _playerStat = _player._playerStat;
        ResentDamage = _playerStat.Strength.GetValue();
        _rightAtkcol = _rightattackRange.GetComponent<CircleCollider2D>();
        leftAtkCol = _range.GetComponentInChildren<PolygonCollider2D>();
        aim = GetComponent<PlayerAim>();
    }

    //public IEnumerator NormalAttack()
    //{
    //    float damage = CalculateDamage(0.8f);
    //    Debug.Log("damage : " + damage);
    //    _player.IsAttacking = false;
    //    leftAtkCol.enabled = true;
    //    EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.LeftAttack, _range.transform.position, 0.3f, GetComponent<PlayerAim>().RotZ, Vector3.right * 0.9f);
    //    yield return new WaitForSeconds(0.1f);
    //    leftAtkCol.enabled = false;
    //    _range.gameObject.SetActive(false);
    //    yield return new WaitForSeconds(1.5f/ (2 + (_playerStat.AttackSpeed.GetValue()+1)));
    //    _player.CanAttack = true;
    //}

    //public IEnumerator ChargingAttack()
    //{
    //    float pressTime = 0;
    //    float factor = 0;

    //    while (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
    //    {
    //        pressTime += 0.05f;
    //        float scale = Mathf.Lerp(0, ChargeTime, pressTime/ChargeTime) / 2 + 1;
    //        _rightattackRange.transform.localScale = new Vector3(scale, scale, 1);
    //        yield return new WaitForSeconds(0.05f);
    //    }

    //    pressTime = Mathf.Clamp(pressTime, 0, ChargeTime);
    //    factor = Mathf.Lerp(0f, 0.7f, pressTime / ChargeTime) + 0.8f;
    //    Debug.Log($"time : {pressTime},  factor : {factor}");

    //    float damage = CalculateDamage(factor);
    //    Debug.Log("damage : " + damage);

    //    _rightAtkcol.enabled = true;
    //    _player.GetComponentInParent<PlayerMovement>().StopImmediately();
    //    _player.ActiveMove = false;
    //    PlayerAim aim = GetComponent<PlayerAim>();
    //    aim.enabled = false;

    //    yield return new WaitForSeconds(0.2f);

    //    EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, _rightattackRange.transform.position, 0.4f);
    //    EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, _rightattackRange.transform.position + Vector3.up / 2, 0.2f);
    //    _rightattackRange.gameObject.SetActive(false);

    //    yield return new WaitForSeconds(0.2f);

    //    _player.CanAttack = false;
    //    _rightAtkcol.enabled = false;
    //    _player.ActiveMove = true;
    //    aim.enabled = true;

    //    yield return new WaitForSeconds(5f / 2 / (_playerStat.AttackSpeed.GetValue()+1));
    //    _player.IsAttacking = false;
    //    _player.CanAttack = true;
    //}

    public IEnumerator NormalAttack()
    {
        float damage = CalculateDamage(0.8f);
        Debug.Log("damage : " + damage);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.RightAttack, _rightattackRange.transform.position + Vector3.up / 3, 0.4f);
        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.MineCustom, _rightattackRange.transform.position + Vector3.up / 2, 0.2f);
        aim.enabled = false;
        _player.ActiveMove = false;
        _rightAtkcol.enabled = true;
        yield return new WaitForSeconds(0.3f);
        aim.enabled = true;
        _player.ActiveMove = true;
        _rightAtkcol.enabled = false;
        _rightattackRange.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f / (2 + (_playerStat.AttackSpeed.GetValue() + 1)));
        _player.CanAttack = true;
        _player.IsAttacking = false;
    }

    public IEnumerator ChargingAttack()
    {
        float pressTime = 0;
        float factor = 0;

        while (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            pressTime += 0.05f;
            float scale = Mathf.Lerp(0, ChargeTime, pressTime / ChargeTime) / 2 + 1;
            _range.transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.05f);
        }

        pressTime = Mathf.Clamp(pressTime, 0, ChargeTime);
        factor = Mathf.Lerp(0f, 0.7f, pressTime / ChargeTime) + 0.8f;
        Debug.Log($"time : {pressTime},  factor : {factor}");

        float damage = CalculateDamage(factor);
        Debug.Log("damage : " + damage);

        leftAtkCol.enabled = true;
        _player.GetComponentInParent<PlayerMovement>().StopImmediately();
        _player.ActiveMove = false;
        PlayerAim aim = GetComponent<PlayerAim>();
        aim.enabled = false;

        yield return new WaitForSeconds(0.3f);

        EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.LeftAttack, _range.transform.position, 0.3f, GetComponent<PlayerAim>().RotZ, Vector3.right * 0.9f);
        _range.gameObject.SetActive(false);

        _player.CanAttack = false;
        leftAtkCol.enabled = false;
        _player.ActiveMove = true;
        aim.enabled = true;

        yield return new WaitForSeconds(4f / 2 / (_playerStat.AttackSpeed.GetValue() + 1));
        _player.IsAttacking = false;
        _player.CanAttack = true;
    }

    [Obsolete]
    private void Update()
    {

        if (_player._isdodgeing)
        {
            if(stopCoroutine != null)
                StopCoroutine(stopCoroutine);
            leftAtkCol.enabled = false;
            _rightAtkcol.enabled = false;
            _range.gameObject.SetActive(false);
            _rightattackRange.SetActive(false);
            _player.IsAttacking = false;
            _player.CanAttack = true;
            return;
        }

        if (Input.GetMouseButton(0) && _player.CanAttack && !_rightattackRange.gameObject.active && !_player.IsAttacking)
        {
            _rightattackRange.gameObject.SetActive(true);
            _player.IsAttacking = true;
        }
        else if(Input.GetMouseButtonUp(0) && _player.CanAttack)
        {
            _player.CanAttack = false;
            stopCoroutine = StartCoroutine(NormalAttack());
        }

        if (Input.GetMouseButtonDown(1) && !_player.IsAttacking && _player.CanAttack)
        {
            _player.IsAttacking = true;
            stopCoroutine = StartCoroutine(ChargingAttack());
            _range.gameObject.SetActive(true);
        }

    }

    public float CalculateDamage(float factor)
    {
        float damage = 0;
        bool critical = false;
        if (UnityEngine.Random.Range(0, 100) < _player._playerStat.Lucky.GetValue())
        {
            damage = ResentDamage * 1.3f * factor;
            critical = true;
        }
        else
        {
            damage = _player._playerStat.Strength.GetValue() * factor;
        }
        ResentDamage = Mathf.Ceil(damage * 10) / 10;
        isCritical = critical;
        Debug.Log("damage : " + damage);
        return damage;
    }

}
