using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombSkill : PlayerSkillAttack
{
    [SerializeField]
    private float bombtime = 3f;

    private float lastSkillusedTime;
    private bool skillused = false;

    private Coroutine coroutine;

    protected override void TryAttack()
    {
        if (skillused && Time.time - lastSkillusedTime <= 3f)
        {
            InvokeSkill();
            StopCoroutine(coroutine);
            coroutine = null;
        }
        else
        {
            base.TryAttack();
        }
        //do something
        coroutine = StartCoroutine(InvokeSkillWithWait());
    }

    IEnumerator InvokeSkillWithWait()
    {
        yield return new WaitForSeconds(bombtime);
        InvokeSkill();
    }

    private void InvokeSkill()
    {
        lastSkillusedTime = 0f;
        skillused = false;

        //스킬 이펙트와 콜라이더 구현

    }

}
