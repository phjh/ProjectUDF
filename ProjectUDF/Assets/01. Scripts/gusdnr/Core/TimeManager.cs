using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    public float NowTime;
    [SerializeField] private float MaxTime;

    [SerializeField]TMP_Text TimerText;
    private bool isWorkingTimer;
    public bool IsWorkingTimer => isWorkingTimer;

	private void Start()
	{
		ResetTimer();
	}

	public void ResetTimer()
    {
        NowTime = MaxTime;
    }

    public void StopTimer(bool isReset = true, bool isStop = true) //Ÿ�̸� ���� (������ ���ΰ�? �����ϴ� ���ΰ�?)
    {
        isWorkingTimer = !isStop;
        if(isReset) { ResetTimer(); }
    }

    public void StartTimer() //�ܺ� ȣ��� Ÿ�̸� ���� �Լ�
    {
        isWorkingTimer = true;
        if(!isWorkingTimer) StartCoroutine(WorkingTimer());
    }

    private IEnumerator WorkingTimer() //Ÿ�̸ӿ� �ڷ�ƾ
    {
        while(NowTime > 0 && isWorkingTimer)
        {
            if(IsWorkingTimer == false) yield break;
			NowTime -= Time.deltaTime;
			DisplayTime(NowTime);

            if(NowTime <= 0)
            {
                StopTimer();
            }
        }
		StopTimer(true);
		yield return null;
    }

	private void DisplayTime(float timeToDisplay) //������ �ð� �ؽ�Ʈȭ
	{
		var t0 = (int)timeToDisplay; //��ü �ð� ��Ʈ�� �ޱ�
		var m = t0 / 60; //�� ���� ��
		var s = (t0 - m * 60); //�� ���� ��
		var ms = (int)((timeToDisplay - t0) * 100); //�и������� �� 2�ڸ�

		TimerText.text = $"[{m:00}:{s:00}:{ms:00}]";
	}
}
