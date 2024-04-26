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

	[Header("BGM")]
	[SerializeField]
	private AudioSource bgmPlayer = null;
	[SerializeField]
	private List<AudioClips> bgmClips;

	[Header("SFX")]
	[SerializeField]
	private SFXSource sfxPlayerObj;
	public int sfxPlayerCount;
	[SerializeField]
	private List<AudioClips> sfxClips;
	private List<SFXSource> sfxPlayers = null;
	

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

		if(bgmPlayer.loop != true) bgmPlayer.loop = true;
		if(bgmPlayer.playOnAwake == true) bgmPlayer.playOnAwake = false;
		InitSFXPlayer();
	}

	#region Methods

	public void StopAllAudio()
	{
		StopBGM();
		StopAllSFX();
	}

	#region BGM
	public void PlayBGM(string name, float pitch = 1.0f)
	{
		AudioClip audioClip = GetAudioClip(name, SoundType.Bgm);
		if (audioClip != null)
		{
			AudioSource audioSource = bgmPlayer;
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.Play();
		}
		else
		{
			Debug.LogError($"Error : There is No name BGM [{name}]");
		}
	}

	public void StopBGM()
	{
		bgmPlayer.Stop();
	}
	#endregion

	#region SFX
	public void PlaySFX(string name, Transform playPoint, float volume = .5f, float pitch = 1.0f)
	{
		AudioClip audioClip = GetAudioClip(name, SoundType.Effect);
		if (audioClip != null)
		{
			for (int p = 0; p < sfxPlayers.Count; p++)
			{
				if (!sfxPlayers[p].IsPlaying)
				{
					sfxPlayers[p].PopAudio(audioClip, playPoint, volume, pitch);
				}
			}
			Debug.LogWarning("All Audio Player're Running!");
			SFXSource newPlayer = MakeSFXPlayer();
			newPlayer.PopAudio(audioClip, playPoint, volume, pitch);
			return;
		}
		else
		{
			Debug.LogError($"Error : There is No name SFX [{name}]");
			return;
		}
	}

	private void InitSFXPlayer()
	{
		for (int c = 0; c < sfxPlayerCount; c++)
		{
			MakeSFXPlayer();
		}
	}

	private SFXSource MakeSFXPlayer()
	{
		SFXSource source = Instantiate(sfxPlayerObj);
		source.transform.SetParent(transform);
		source.transform.position = Vector2.zero;
		AudioSource audio = source.audioPlayer;
		if (audio == null)
		{
			source.gameObject.AddComponent<AudioSource>();
			audio = source.GetSource();
		}

		if(audio != null)
		{
			sfxPlayers.Add(source);
			sfxPlayerCount = sfxPlayers.Count;
		}
		return source;
	}

	public void StopAllSFX() //모든 음향 효과 재생 종료
	{
		// 재생기 전부 재생 스탑, 음반 빼기
		foreach (SFXSource audioSource in sfxPlayers)
		{
			audioSource.audioPlayer.clip = null;
			audioSource.PushAudio();
		}
		// 효과음 Dictionary 비우기
	}
	#endregion

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

