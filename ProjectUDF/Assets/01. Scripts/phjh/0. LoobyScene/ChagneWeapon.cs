using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChagneWeapon : MonoBehaviour
{
    public PlayerWeapon weapon;

    GameObject keysprite;
    SpriteRenderer sp;
    Collider2D collider;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        keysprite.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerMain.Instance.SetWeapon(weapon);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }


}
