using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	[SerializeField] private float baseValue;
	[HideInInspector] public float realValue;
	public List<float> fixedModifiers = new List<float>();
	public List<float> persentedModifiers = new List<float>();

	public void ResetValue() //���� ������ �ʱⰪ���� �� ���ÿ� ����
	{
		realValue = baseValue;
	}

	public float GetValue()
	{
		float finalValue = realValue;
		for (int i = 0; i < fixedModifiers.Count; ++i)
		{
			finalValue += fixedModifiers[i];
		}
		for (int i = 0; i < persentedModifiers.Count; ++i)
		{
			finalValue *= 1 + persentedModifiers[i] * 0.01f;
		}
		return finalValue;
	}

	public void SetRealValue(float finalValue) //������ �������� �ʴ� �� ���� �� ����
	{
		realValue = finalValue;
	}

	public void SetDefaultValue(float value) //���� ���۽� ���� �ʱ� �� ����
	{
		baseValue = value;
		realValue = baseValue;
	}

	public void AddModifier(float value, bool IsPersent = false) //Stat �ɷ�ġ ���� ��ġ�� Modifiers ����Ʈ�� �߰��� �� ����
	{
		if (value != 0)
		{
			if (IsPersent){ fixedModifiers.Add(value); }
			else { persentedModifiers.Add(value); }
		}
	}

	public void RemoveModifier(float value, bool IsPersent = false) //Modifiers ����Ʈ���� value ���� ������ �� �����ϱ�
	{
		if (value != 0)
		{
			if (IsPersent) { fixedModifiers.Remove(value); }
			else { persentedModifiers.Remove(value); }
		}
	}
}
