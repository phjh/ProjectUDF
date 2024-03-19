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
        float damage = _playerStat.Strength.GetValue() * 4/5;
        if(UnityEngine.Random.Range(0,100) < _playerStat.Lucky.GetValue())
        {
            damage = ResentDamage * 1.3f;
            Debug.Log("!!!!!!damage : " + damage);
        }
        Debug.Log("damage : " + damage);
        ResentDamage = Mathf.Ceil(damage * 10)/10;
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

        float damage;
        if (UnityEngine.Random.Range(0, 100) < _playerStat.Lucky.GetValue())
        {
            damage = ResentDamage * 1.3f * factor;
            Debug.Log("!!!!!!damage : " + damage);
        }
        else
        {
            damage = _playerStat.Strength.GetValue() * factor;
        }
        Debug.Log("damage : " + damage);
        ResentDamage = Mathf.Ceil(damage * 10) / 10;

        _rightAtkcol.enabled = true;
        _player.GetComponentInParent<PlayerMovement>().StopImmediately();
        _player.ActiveMove = false;
        yield return new WaitForSeconds(0.2f);
        _rightattackRange.gameObject.SetActive(false);
        _rightAtkcol.enabled = false;
        _player.ActiveMove = true;
        yield return new WaitForSeconds(5f / 2 / (_playerStat.AttackSpeed.GetValue()+1));
        _player.IsAttacking = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !_player.IsAttacking)
        {
            _player.IsAttacking = true;
            _range.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
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
        else if (Input.GetMouseButtonUp(1))
        {
        }

    }
}
