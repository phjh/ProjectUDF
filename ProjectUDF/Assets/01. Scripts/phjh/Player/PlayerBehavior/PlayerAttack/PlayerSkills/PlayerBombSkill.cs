using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombSkill : PlayerSkillAttack
{
    [SerializeField]
    private float bombtime = 3f;

    private float lastSkillusedTime;
    private bool skillused = false;

    private Coroutine coroutine;

    public Slider Slider;

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
        float time = 0;
        if(time < bombtime)
        {
            Slider.value = time / bombtime;
            yield return new WaitForSeconds(Time.fixedTime);
        }
        InvokeSkill();
        yield return new WaitForSeconds(0.1f);
        EndSkill();
    }

    private void InvokeSkill()
    {
        lastSkillusedTime = 0f;
        skillused = false;

        //��ų ����Ʈ�� �ݶ��̴� ����

    }

    private void EndSkill()
    {

    }

}
