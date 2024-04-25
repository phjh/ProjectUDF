using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

	private void OnEnable()
	{
		if (audioMixer == null)
		{
			AudioMixer resource = Resources.Load<AudioMixer>("MainMixer");
			audioMixer = resource;
		}
	}

	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
	}

	public void SetSoundFXXVolume(float volume)
	{

		audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
	}

	public void SetBGMVolume(float volume)
	{

		audioMixer.SetFloat("bgmVolume", Mathf.Log10(volume) * 20f);
	}
}
