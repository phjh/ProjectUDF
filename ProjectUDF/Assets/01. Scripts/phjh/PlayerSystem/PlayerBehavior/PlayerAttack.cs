using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : Player
{
    [SerializeField] Player _player;
    [SerializeField] PlayerAttackRange _range;
    [SerializeField] GameObject _rightattackRange;

    public float ChargeTime = 1.6f;
    public float stiffenTime = 0.4f; //�����ð�

    bool IsAttacking = false;

    float ResentDamage;

    protected void Start()
    {

        _playerStat = _player._playerStat;
        ResentDamage = _playerStat.PlayerStrength;
    }

    public IEnumerator NormalAttack()
    {
        float damage = _playerStat.PlayerStrength * 4/5;
        if(Random.Range(0,100) < _playerStat.PlayerLucky)
        {
            damage = ResentDamage * 1.3f;
            Debug.Log("!!!!!!damage : " + damage);
        }
        Debug.Log("damage : " + damage);
        ResentDamage = Mathf.Ceil(damage * 10)/10;
        yield return new WaitForSeconds(1.6f/ 2 / _playerStat.PlayerAttackSpeed);
        IsAttacking = false;
    }

    public IEnumerator ChargingAttack()
    {
        float pressTime = 0;
        float factor = 0;

        while (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            pressTime += 0.05f;
            _rightattackRange.transform.localScale = new Vector3(Mathf.Lerp(0, ChargeTime, pressTime) + 1, Mathf.Lerp(0, ChargeTime, pressTime) + 1, 1);
            yield return new WaitForSeconds(0.05f);
        }

        pressTime = Mathf.Clamp(pressTime, 0, ChargeTime);
        factor = Mathf.Lerp(-0.2f, 0.2f, pressTime / ChargeTime) + 1;
        Debug.Log($"time : {pressTime},  factor : {factor}");

        float damage;
        if (Random.Range(0, 100) < _playerStat.PlayerLucky)
        {
            damage = ResentDamage * 1.3f * factor;
            Debug.Log("!!!!!!damage : " + damage);
        }
        else
        {
            damage = _playerStat.PlayerStrength * factor;
        }
        Debug.Log("damage : " + damage);
        ResentDamage = Mathf.Ceil(damage * 10) / 10;

        _player.GetComponentInParent<PlayerMovement>().StopImmediately();
        _player.ActiveMove = false;
        yield return new WaitForSeconds(0.4f);
        _player.ActiveMove = true;
        yield return new WaitForSeconds(5f / 2 / _playerStat.PlayerAttackSpeed);
        IsAttacking = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !IsAttacking)
        {
            IsAttacking = true;
            _range.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StartCoroutine(NormalAttack());
            _range.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1) && !IsAttacking)
        {
            IsAttacking = true;
            StartCoroutine(ChargingAttack());
            _rightattackRange.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _rightattackRange.gameObject.SetActive(false);
        }

    }
}
