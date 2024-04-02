using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMono : MonoBehaviour
{
    private UIManager um = UIManager.Instance;

    public abstract void ShowUI();
    public abstract void CloseUI();
}
