using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.Instance.player.gameObject)
        {
            MapSystem.Instance.OnRoomStart();
            GameManager.Instance.player.transform.parent.transform.position = new Vector2(0,0);
            this.gameObject.SetActive(false);
        }
    }
}
