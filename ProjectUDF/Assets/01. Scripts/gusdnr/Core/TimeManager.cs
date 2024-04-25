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

	public void StartTimer() //외부 호출용 타이머 시작 함수
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

    private IEnumerator WorkingTimer() //타이머용 코루틴
    {
        while(NowTime > 0)
        {
			if (!IsWorkingTimer) // IsWorkingTimer가 false일 때 타이머 종료
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

	private void DisplayTime(float timeToDisplay) //보여줄 시간 텍스트화
	{
		var t0 = (int)timeToDisplay; //전체 시간 인트로 받기
		var m = t0 / 60; //분 단위 값
		var s = (t0 - m * 60); //초 단위 값
		var ms = (int)((timeToDisplay - t0) * 100); //밀리세컨드 앞 2자리
		TimerText.text = $"[ {m:00} : {s:00} : {ms:00} ]";
	}
}
