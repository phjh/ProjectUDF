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
		for (int i = 0; i < fixedModifiers.Count; ++i)
		{
			finalValue += fixedModifiers[i];
		}
		for (int i = 0; i < persentedModifiers.Count; ++i)
		{
			finalValue *= 1 + (persentedModifiers[i] * 0.01f);
		}
		return finalValue;
	}

	public void SetRealValue(float finalValue) //버프가 존재하지 않는 실 적용 값 세팅
	{
		baseValue = finalValue;
	}

	public void SetDefaultValue(float value) //게임 시작시 가장 초기 값 세팅
	{
		baseValue = value;
	}

	public void AddModifier(float value, bool IsPersent = false) //Stat 능력치 증가 수치를 Modifiers 리스트에 추가해 후 적용
	{
		if (value != 0)
		{
			if (IsPersent){ fixedModifiers.Add(value); }
			else { persentedModifiers.Add(value); }
		}
	}

	public void RemoveModifier(float value, bool IsPersent = false) //Modifiers 리스트에서 value 값이 존재할 때 삭제하기
	{
		if (value != 0)
		{
			if (IsPersent) { fixedModifiers.Remove(value); }
			else { persentedModifiers.Remove(value); }
		}
	}
}
