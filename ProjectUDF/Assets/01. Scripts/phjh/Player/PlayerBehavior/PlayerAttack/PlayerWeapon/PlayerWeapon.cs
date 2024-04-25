using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "SO/Player/Weapon")]
public class PlayerWeapon : ScriptableObject
{
    public SelectedWeaponEnum weaponType;
    
    public PlayerBaseAttack baseAtk;
    public PlayerChargeAttack chargeAtk;

    public float GetCaculateDamage()
    {
        return 0;
    }

    private void Awake()
    {
        if(baseAtk == null)
        {
            Debug.LogError($"{this.name} baseAtk is null!");
        }
        else if(chargeAtk == null)
        {
            Debug.LogError($"{this.name} chargeAtk is null!");
        }
    }

}
