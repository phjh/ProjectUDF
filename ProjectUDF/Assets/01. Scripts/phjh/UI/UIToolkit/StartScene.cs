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
        VisualElement root = _uiDocument.rootVisualElement;
        //이녀석은 UI Object다 . 모든 애들의 근간이 된다.

        Button btn = root.Q<Button>("StartBtn");
        //Query => 질의, 질문

        btn.RegisterCallback<ClickEvent>(e =>
        {
            SceneManager.LoadScene((int)NextScene);
        });


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            SceneManager.LoadScene((int)NextScene);

    }

}
