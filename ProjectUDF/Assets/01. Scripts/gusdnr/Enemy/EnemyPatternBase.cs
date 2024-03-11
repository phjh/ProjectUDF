using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour
{

	public abstract void Init();
    public abstract void ResetValue();
    public abstract void Action();
}
