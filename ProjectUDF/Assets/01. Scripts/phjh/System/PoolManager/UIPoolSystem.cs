using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPoolSystem : MonoSingleton<UIPoolSystem>
{
    public void PopupDamageText(PoolUIListEnum type, float strength, float damage, float time, Vector3 trm, bool isCritical = false) => StartCoroutine(DamageTextSetting(type, strength, damage, time, trm, isCritical));

    private IEnumerator DamageTextSetting(PoolUIListEnum type, float strength, float damage, float time, Vector3 trm, bool isCritical)
    {
        Debug.Log(0);
        PoolableMono mono = PoolManager.Instance.Pop(type);
        mono.transform.position = trm + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0);
        mono.transform.DOMoveY(mono.transform.position.y + Random.Range(time / 2, time) + 1, time).SetEase(Ease.OutCirc);
        Debug.Log(1);
        TextMeshPro tmp = mono.GetComponent<TextMeshPro>();
        tmp.text = damage.ToString();
        Debug.Log(2);
        tmp.fontSize = 4;
        if (isCritical)
        {
            time += 0.2f;
            tmp.fontSize *= 1.25f;
            tmp.color = Color.red;
        }
        else
        {
            tmp.color = Color.white;
            tmp.fontSize = 4;
        }
        Debug.Log(3);
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Push(mono, type);
    }


}
