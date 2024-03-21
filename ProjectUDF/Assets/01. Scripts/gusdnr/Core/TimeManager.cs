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

    public void StopTimer(bool isReset = true, bool isStop = true) //타이머 정지 (리셋할 것인가? 정지하는 것인가?)
    {
        isWorkingTimer = !isStop;
        if(isReset) { ResetTimer(); }
    }

    public void StartTimer() //외부 호출용 타이머 시작 함수
    {
        isWorkingTimer = true;
        if(!isWorkingTimer) StartCoroutine(WorkingTimer());
    }

    private IEnumerator WorkingTimer() //타이머용 코루틴
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

	private void DisplayTime(float timeToDisplay) //보여줄 시간 텍스트화
	{
		var t0 = (int)timeToDisplay; //전체 시간 인트로 받기
		var m = t0 / 60; //분 단위 값
		var s = (t0 - m * 60); //초 단위 값
		var ms = (int)((timeToDisplay - t0) * 100); //밀리세컨드 앞 2자리

		TimerText.text = $"[{m:00}:{s:00}:{ms:00}]";
	}
}
