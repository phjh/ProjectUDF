using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.Instance.player.gameObject)
        {
            MapSystem.Instance.OnPortalEnter(this.transform, GameManager.Instance.player.transform.parent.transform);
            MapSystem.Instance.OnRoomStart();
            this.gameObject.SetActive(false);
        }
    }
}
