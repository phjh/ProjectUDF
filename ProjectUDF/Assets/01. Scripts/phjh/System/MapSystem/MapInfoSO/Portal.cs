using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.Instance.player.gameObject)
        {
            MapSystem.Instance.ActionInvoker(MapEvents.MapStart);
            GameManager.Instance.player.gameObject.transform.position = new Vector2(15, 9.5f);
            this.gameObject.SetActive(false);
        }
    }
}
