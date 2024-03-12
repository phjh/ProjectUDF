using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Player
{
    [SerializeField] Player _player;
    [SerializeField] PlayerAttackRange _range;

    float ResentDamage;

    protected void Start()
    {

        _playerStat = _player._playerStat;
        ResentDamage = _playerStat.PlayerStrength;
    }

    public float Attack()
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

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _range.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Attack();
            _range.gameObject.SetActive(false);
        }

    }


}
