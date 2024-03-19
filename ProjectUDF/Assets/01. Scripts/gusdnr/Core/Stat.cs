using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	[SerializeField] private float baseValue;
	public List<float> fixedModifiers = new List<float>();
	public List<float> persentedModifiers = new List<float>();

	public float GetValue()
	{
		float finalValue = baseValue;
		for (int i = 0; i < fixedModifiers.Count; i++)
		{
			finalValue += fixedModifiers[i];
		}
		for (int i = 0; i < persentedModifiers.Count; i++)
		{
			finalValue *= 1 + (persentedModifiers[i] * 0.01f);
		}
		return finalValue;
	}

	public void SetRealValue(float finalValue) //Set None Buff Stat value
	{
		baseValue = finalValue;
	}

	public void SetDefaultValue(float value) //When Game Started, Set DefalutValue
	{
		baseValue = value;
	}

	public void AddModifier(float value, bool IsPersent = false) //Add Stat Modifier Values
	{
		if (value != 0)
		{
			if (IsPersent){ fixedModifiers.Add(value); }
			else { persentedModifiers.Add(value); }
		}
	}

	public void RemoveModifier(float value, bool IsPersent = false) //Modifiers Remoce to value in list
	{
		if (value != 0)
		{
			if (IsPersent) { fixedModifiers.Remove(value); }
			else { persentedModifiers.Remove(value); }
		}
	}
}
