using GameManageDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    private UIDocument _uiDocument;
    public SceneList NextScene;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
            return;

        VisualElement root = _uiDocument.rootVisualElement;
        //�̳༮�� UI Object�� . ��� �ֵ��� �ٰ��� �ȴ�.

        Button btn = root.Q<Button>("StartBtn");
        //Query => ����, ����

        btn.RegisterCallback<ClickEvent>(e =>
        {
            SceneManager.LoadScene((int)NextScene);
        });


    }

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
