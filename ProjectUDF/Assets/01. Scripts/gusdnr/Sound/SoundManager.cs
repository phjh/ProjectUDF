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
	AudioSource[] audioSources = new AudioSource[(int)SoundType.MaxCount];
	[SerializeField]
	private List<AudioClips> BgmClip;
	[SerializeField]
	private List<AudioClips> EffectClip;

	Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

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

		audioSources[(int)SoundType.Bgm].loop = true;
	}

	#region Methods

	public void Play(string name, SoundType type = SoundType.Effect, float volume = 0.5f, float pitch = 1.0f)
	{
		AudioClip audioClip = GetAudioClip(name, type);

		switch (type)
		{
			case SoundType.Bgm: // BGM 배경음악 재생
				{
					AudioSource audioSource = audioSources[(int)SoundType.Bgm];
					if (audioSource.isPlaying)
						audioSource.Stop();

					audioSource.clip = audioClip;
					audioSource.pitch = pitch;
					audioSource.volume = volume;
					audioSource.Play();
					break;
				}
			case SoundType.Effect: // Effect 효과음 재생
				{
					AudioSource audioSource = audioSources[(int)SoundType.Effect];
					audioSource.volume = volume;
					audioSource.pitch = pitch;
					audioSource.PlayOneShot(audioClip);
					break;
				}
			case SoundType.MaxCount:
			default:
				{
					Debug.LogError("Clip has Wrong Type");
					break;
				}
		}
	}

	public void Clear()
	{
		// 재생기 전부 재생 스탑, 음반 빼기
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.clip = null;
			audioSource.Stop();
		}
		// 효과음 Dictionary 비우기
		audioClips.Clear();
	}


	public AudioClip GetAudioClip(string name, SoundType type = SoundType.Effect) //SoundType과 이름을 바탕으로 해당하는 Sound Clip 할당 받기
	{
		List<AudioClips> currentClipList = null;

		switch (type)
		{
			case SoundType.Bgm: currentClipList = BgmClip; break;
			case SoundType.Effect: currentClipList = EffectClip; break;
			case SoundType.MaxCount:
			default:
				{
					Debug.LogError("Clip List Is Missing!");
					break;
				}
		}

		AudioClip audioClip = currentClipList.Find(c => c.ID.Equals(name)).clip;
		return audioClip;
	}

	#endregion
}

