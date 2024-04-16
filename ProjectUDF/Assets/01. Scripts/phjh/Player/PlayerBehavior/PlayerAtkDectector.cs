using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerAtkDectector : MonoBehaviour
{
    [SerializeField] PlayerAttack _playerAtk;
    CinemachineBasicMultiChannelPerlin perlin;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<EnemyMain>(out EnemyMain enemy))
        {
            Debug.Log("Connected trigger damage : " + _playerAtk.ResentDamage);
            EffectSystem.Instance.EffectInvoker(PoolEffectListEnum.HitEffect, transform.position + (collision.gameObject.transform.position - transform.position) / 2, 0.3f);
            UIPoolSystem.Instance.PopupDamageText(PoolUIListEnum.DamageText, _playerAtk._player._playerStat.Strength.GetValue(), _playerAtk.ResentDamage, 0.5f, collision.transform.position);
            StartCoroutine(ShakeCamera());
            enemy.Damage(_playerAtk.ResentDamage);
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
    
    IEnumerator ShakeCamera()
    {
        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 1;
        perlin.m_FrequencyGain = 1;
        yield return new WaitForSeconds(0.2f);
        perlin = GameManager.Instance.VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }


}
