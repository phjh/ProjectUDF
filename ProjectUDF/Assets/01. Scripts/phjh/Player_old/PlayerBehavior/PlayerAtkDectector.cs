using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    CinemachineBasicMultiChannelPerlin perlin;

    private void Start()
    {
        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            Debug.Log("Connected trigger damage : " + PlayerMain.Instance.recentDamage);
            EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, PlayerMain.Instance.stat.Strength.GetValue(), PlayerMain.Instance.recentDamage, 0.5f, collision.transform.position, PlayerMain.Instance.isCritical);
            enemy.Damage(PlayerMain.Instance.recentDamage);
            StartCoroutine(ShakeCamera(PlayerMain.Instance.recentDamage, PlayerMain.Instance.isCritical));
        }
        else if(collision.CompareTag("Enemy"))
        {
            Debug.Log("¿Ã∞≈ ø÷∂‰?");
        }
        else
        {
            Debug.Log($"collisoin : {collision}");
        }
    }
    
    IEnumerator ShakeCamera(float shakeIntencity, bool iscritical)
    {
        float frequency = 1f;
        if (iscritical)
        {
            shakeIntencity *= 1.2f;
            frequency += 0.5f;
        }
        perlin.m_AmplitudeGain = shakeIntencity * 0.5f;
        perlin.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(0.2f);
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }


}
