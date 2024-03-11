using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatternBase
{
    public abstract void ResetValue();
	public abstract void InitPattern();
    public abstract void ActionPattern();
    public abstract void EndPattern();
}
