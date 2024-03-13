using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Player
{
    [SerializeField] Player _player;
    [SerializeField] PlayerAttackRange _range;
    [SerializeField] GameObject _rightattackRange;

    public float ChargeTime = 1.6f;

    float ResentDamage;

    protected void Start()
    {

        _playerStat = _player._playerStat;
        ResentDamage = _playerStat.PlayerStrength;
    }

    public float NormalAttack()
    {
        float damage = _playerStat.PlayerStrength;
        if(Random.Range(0,100) < _playerStat.PlayerLucky)
        {
            damage = ResentDamage * 1.3f;
            Debug.Log("!!!!!!damage : " + damage);
        }
        Debug.Log("damage : " + damage);
        ResentDamage = Mathf.Ceil(damage * 10)/10;
        return damage;
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
        factor = Mathf.Lerp(0f, 0.4f, pressTime * 5 / 4) + 1;
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

        stopImmediately?.Invoke();
        _player.ActiveMove = false;
        yield return new WaitForSeconds(0.2f);
        _player.ActiveMove = true;


    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _range.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            NormalAttack();
            _range.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ChargingAttack());
            _rightattackRange.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _rightattackRange.gameObject.SetActive(false);
        }
    }


}
