using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPoolSystem : MonoSingleton<UIPoolSystem>
{
    public bool isCritical = false;

    public void PopupDamageText(PoolUIListEnum type, float strength, float damage, float time, Transform trm) => StartCoroutine(DamageTextSetting(type, strength, damage, time, trm));


    private IEnumerator DamageTextSetting(PoolUIListEnum type, float strength, float damage, float time, Transform trm)
    {
        Debug.Log(0);
        PoolableMono mono = PoolManager.Instance.Pop(type);
        mono.transform.position = trm.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
        mono.transform.DOMoveY(transform.position.y + Random.Range(time / 2, time * 2), Random.Range(time, time * 3));
        Debug.Log(1);
        TextMeshPro tmp = mono.GetComponent<TextMeshPro>();
        tmp.text = damage.ToString();
        Debug.Log(2);
        tmp.fontSize = 3 + damage / strength;
        if (isCritical)
        {
            time += 0.2f;
            tmp.fontSize += 0.5f;
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
