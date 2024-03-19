using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private PanelHandler[] Cards;

    public void ShowCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].Show();
        }
    }
}
