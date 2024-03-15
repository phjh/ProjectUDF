using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	[SerializeField] private float baseValue;
	[HideInInspector] public float realValue;
	public List<float> modifiers = new List<float>();

	public float GetValue()
	{
		float finalValue = realValue;
		for (int i = 0; i < modifiers.Count; ++i)
		{
			finalValue += modifiers[i];
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

	public void AddModifier(float value) //Stat �ɷ�ġ ���� ��ġ�� Modifiers ����Ʈ�� �߰��� �� ����
	{
		if (value != 0)
			modifiers.Add(value);
	}

	public void RemoveModifier(float value) //Modifiers ����Ʈ���� value ���� ������ �� �����ϱ�
	{
		if (value != 0)
			modifiers.Remove(value);
	}
}
