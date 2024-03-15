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

	public void SetRealValue(float finalValue) //버프가 존재하지 않는 실 적용 값 세팅
	{
		realValue = finalValue;
	}

	public void SetDefaultValue(float value) //게임 시작시 가장 초기 값 세팅
	{
		baseValue = value;
		realValue = baseValue;
	}

	public void AddModifier(float value) //Stat 능력치 증가 수치를 Modifiers 리스트에 추가해 후 적용
	{
		if (value != 0)
			modifiers.Add(value);
	}

	public void RemoveModifier(float value) //Modifiers 리스트에서 value 값이 존재할 때 삭제하기
	{
		if (value != 0)
			modifiers.Remove(value);
	}
}
