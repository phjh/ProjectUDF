using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioClips
{
	public string ID;
	public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
	public enum SoundType
	{
		Bgm = 0,
		Effect = 1,
		MaxCount
	}

	public static SoundManager Instance;

	[SerializeField]
	private AudioSource bgmPlayer = null;
	[SerializeField]
	private AudioSource[] sfxPlayers = null;
	[SerializeField]
	private List<AudioClips> bgmClips;
	[SerializeField]
	private List<AudioClips> sfxClips;

	private void OnEnable()
	{
		if(Instance != null)
		{
			if(bgmPlayer == null) bgmPlayer = transform.Find("BGMPlayer").GetComponent<AudioSource>();
			if(sfxPlayers == null) sfxPlayers = transform.Find("SFXPlayer").GetComponents<AudioSource>();
		}
	}

	private void Awake()
	{
		var obj = FindAnyObjectByType<SoundManager>();
		if (obj != null)
		{
			Destroy(obj);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		bgmPlayer.loop = true;
	}

	#region Methods

	public void PlayBGM(string name, float volume = 0.5f, float pitch = 1.0f)
	{
		AudioClip audioClip = GetAudioClip(name, SoundType.Bgm);
		if (audioClip != null)
		{
			AudioSource audioSource = bgmPlayer;
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.volume = volume;
			audioSource.Play();
		}
		else
		{
			Debug.LogError($"Error : There is No name BGM [{name}]");
		}
	}

	public void PlaySFX(string name, float volume = 0.5f, float pitch = 1.0f)
	{
		AudioClip audioClip = GetAudioClip(name, SoundType.Effect);
		if (audioClip != null)
		{
			for (int p = 0; p < sfxPlayers.Length; p++)
			{
				if (!sfxPlayers[p].isPlaying)
				{
					sfxPlayers[p].volume = volume;
					sfxPlayers[p].pitch = pitch;
					sfxPlayers[p].PlayOneShot(audioClip);
					return;
				}
			}
			Debug.LogError("Error : All Audio Player're Running!");
			return;
		}
		else
		{
			Debug.LogError($"Error : There is No name SFX [{name}]");
			return;
		}
	}

	public void StopAllSFX()
	{
		// 재생기 전부 재생 스탑, 음반 빼기
		foreach (AudioSource audioSource in sfxPlayers)
		{
			audioSource.clip = null;
			audioSource.Stop();
		}
		// 효과음 Dictionary 비우기
	}

	public void StopBGM()
	{
		bgmPlayer.Stop();
	}

	public AudioClip GetAudioClip(string name, SoundType type = SoundType.Effect) //SoundType과 이름을 바탕으로 해당하는 Sound Clip 할당 받기
	{
		List<AudioClips> currentClipList = null;

		switch (type)
		{
			case SoundType.Bgm: currentClipList = bgmClips; break;
			case SoundType.Effect: currentClipList = sfxClips; break;
			case SoundType.MaxCount:
			default:
				{
					Debug.LogError("Clip List Is Missing!");
					break;
				}
		}

		AudioClip audioClip = currentClipList.Find(c => c.ID.Equals(name)).clip;
		Debug.Assert(audioClip != null, $"Error : There is No name SFX {name}");
		return audioClip;
	}

	#endregion
}

