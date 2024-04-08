using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMono : MonoBehaviour
{
    [Header("UI Mono Default Values")]
    public int OrderInUI = 0;
    public bool isBased = false;

    public abstract void ShowUI();
    public abstract void CloseUI();
}
