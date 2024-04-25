using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    public float NowTime;
    [SerializeField] private float MaxTime;

    public TMP_Text TimerText;
    private bool isWorkingTimer;
    public bool IsWorkingTimer
    {
        get { return  isWorkingTimer; }
        set { isWorkingTimer = value; }
    }

	public event Action OnTimerEnd;

	private Coroutine timerCoroutine;

	private void OnEnable()
	{
		PlayerStats.OnDeadPlayer += HideTimer;
		GameManager.OnPlaying += StartTimer;
		GameManager.OnPauseUI += StopTimer;
	}

	private void OnDisable()
	{
		PlayerStats.OnDeadPlayer -= HideTimer;
		GameManager.OnPlaying -= StartTimer;
		GameManager.OnPauseUI -= StopTimer;
	}

	private void Awake()
	{
		ResetTimer();
	}

	public void ResetTimer()
	{
		StopTimer();
		NowTime = MaxTime;
		TimerText.gameObject.SetActive(true);
		DisplayTime(NowTime);
	}

	public void StopTimer()
	{
		IsWorkingTimer = false;
		if(timerCoroutine != null) StopCoroutine(timerCoroutine);
	}

	public void StartTimer() //�ܺ� ȣ��� Ÿ�̸� ���� �Լ�
    {
		if (!IsWorkingTimer)
		{
			IsWorkingTimer = true;
			timerCoroutine = StartCoroutine(WorkingTimer());
		}
	}

	public void HideTimer()
	{
		StopTimer();
		TimerText.gameObject.SetActive(false);
	}

    private IEnumerator WorkingTimer() //Ÿ�̸ӿ� �ڷ�ƾ
    {
        while(NowTime > 0)
        {
			if (!IsWorkingTimer) // IsWorkingTimer�� false�� �� Ÿ�̸� ����
			{
				ResetTimer();
				yield break;
			}

			NowTime -= Time.deltaTime;
			DisplayTime(NowTime);

            yield return null;
		}
		OnTimerEnd?.Invoke();
		GameManager.Instance.UpdateResult(GameResults.TimeOut);
		StopTimer();
    }

	private void DisplayTime(float timeToDisplay) //������ �ð� �ؽ�Ʈȭ
	{
		var t0 = (int)timeToDisplay; //��ü �ð� ��Ʈ�� �ޱ�
		var m = t0 / 60; //�� ���� ��
		var s = (t0 - m * 60); //�� ���� ��
		var ms = (int)((timeToDisplay - t0) * 100); //�и������� �� 2�ڸ�
		TimerText.text = $"[ {m:00} : {s:00} : {ms:00} ]";
	}
}
