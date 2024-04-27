using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : MonoBehaviour
{
    public enum BossState
    {
        None = 0,
        Cooldown = 1,
        Attack = 2,
        InCC = 3,
		Died = 4,
    }

    public BossDataSO BossData;
    public BossState CurBossState;
    [Range(0, 10)] public float PatternTerm;
    public float CurHP;
    public bool IsAlive;

    public BossPassive[] PassivePattterns;
    public BossPattern[] ActivePattterns;


    public void StartPassivePattern(BossPassive passive)
    {
        float tickTime = passive.ActiveTickTime;
        StartCoroutine(StartPassive(tickTime, passive));
    }

    private IEnumerator StartPassive(float tick, BossPassive passive)
    {
        while (IsAlive)
        {
            passive?.PaasiveActive();
			yield return new WaitForSeconds(tick);
        }
    }
}
