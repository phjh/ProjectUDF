using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "SO/Player/Weapon")]
public class PlayerWeapon : ScriptableObject
{
    public SelectedWeaponEnum weaponType;

    public GameObject weaponObj;

    public List<AnimationReferenceAsset> leftAttackAnimations;

    public List<AnimationReferenceAsset> chargingAttack;

    public List<AnimationReferenceAsset> rightAttackAnimations;

    public List<AnimationReferenceAsset> WeaponIdleAnimations;

    public static List<string> ToStringArray(List<AnimationReferenceAsset> asset)
    {
        List<string> str = new();
        foreach(var item in asset)
        {
            str.Add(item.ToString());
        }
        return str;
    }

    public float GetCaculateDamage()
    {
        return 0;
    }

}
