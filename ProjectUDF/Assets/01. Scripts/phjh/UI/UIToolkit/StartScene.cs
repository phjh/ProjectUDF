using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    private UIDocument _uiDocument;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();

    }

    private void OnEnable()
    {
        VisualElement root = _uiDocument.rootVisualElement;
        //�̳༮�� UI Object�� . ��� �ֵ��� �ٰ��� �ȴ�.

        Button btn = root.Q<Button>("StartBtn");
        //Query => ����, ����

        btn.RegisterCallback<ClickEvent>(e =>
        {
            SceneManager.LoadScene((int)SceneList.LobbyScene);
        });


    }
}
