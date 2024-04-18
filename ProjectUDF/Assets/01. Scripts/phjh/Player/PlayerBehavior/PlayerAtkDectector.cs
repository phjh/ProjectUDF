using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    [SerializeField] PlayerAttack _playerAtk;
    CinemachineBasicMultiChannelPerlin perlin;

    private void Start()
    {
        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            Debug.Log("Connected trigger damage : " + _playerAtk.ResentDamage);
            EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, _playerAtk._player._playerStat.Strength.GetValue(), _playerAtk.ResentDamage, 0.5f, collision.transform.position,_playerAtk.isCritical);
            enemy.Damage(_playerAtk.ResentDamage);
            StartCoroutine(ShakeCamera( _playerAtk.ResentDamage, _playerAtk.isCritical));
        }
        else if(collision.CompareTag("Enemy"))
        {
            Debug.Log("�̰� �ֶ�?");
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
        yield return new WaitForSeconds(0.25f);
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }


}
