using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChagneWeapon : MonoBehaviour
{
    public PlayerWeapon weapon;

    SpriteRenderer sp;
    Collider2D collider;
    public GameObject keysprite;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        keysprite.transform.position = this.transform.position + Vector3.up / 2f;
        keysprite.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerMain.Instance.SetWeapon(weapon);
            LobbyToGame.Instance.SetNowWeapon(weapon);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.F))
        {
            PlayerMain.Instance.SetWeapon(weapon);
            LobbyToGame.Instance.SetNowWeapon(weapon);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            keysprite.gameObject.SetActive(false);
    }


}
