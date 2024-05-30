using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyToGame : MonoSingleton<LobbyToGame>
{
    public PlayerWeapon nowWeapon;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetNowWeapon(PlayerWeapon weapon)
    {
        nowWeapon = weapon;
    }

    public PlayerWeapon GetnowWeapon()
    {
        return nowWeapon;
    }

    public void DeleteThis()
    {  
        Destroy(gameObject);
    }

}
