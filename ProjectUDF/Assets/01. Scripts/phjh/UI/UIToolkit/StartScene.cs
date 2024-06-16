using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    public SceneList NextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerMain.Instance.nowWeapon != null)
        {
            SceneManager.LoadScene((int)NextScene);
        }
        else
        {
            //���⸦ �����϶�� �޽��� ���
        }
    }

}
