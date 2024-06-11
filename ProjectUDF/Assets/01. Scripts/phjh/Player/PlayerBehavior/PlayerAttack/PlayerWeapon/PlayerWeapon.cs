using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "SO/Player/Weapon")]
public class PlayerWeapon : ScriptableObject
{
    public SelectedWeaponEnum weaponType;

    public GameObject weaponObj;

    public List<AnimationReferenceAsset> WeaponIdleAnimations;

    public List<AnimationReferenceAsset> AdditionalAnimations;


    public float GetCaculateDamage()
    {
        return 0;
    }

}
