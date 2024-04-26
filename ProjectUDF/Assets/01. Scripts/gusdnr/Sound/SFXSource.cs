using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXSource : MonoBehaviour
{
    public AudioSource audioPlayer;
    public bool IsPlaying {  get; private set; }
    private float clipLength = 0f;

    public AudioSource GetSource()
    {
        audioPlayer = GetComponent<AudioSource>();
        return audioPlayer;
    }

    public void InitObject()
    {
        if(audioPlayer == null) audioPlayer = GetComponent<AudioSource>();
        audioPlayer.playOnAwake = false;
        audioPlayer.loop = false;
		Debug.Assert(audioPlayer.outputAudioMixerGroup != null, "Audio Mixer Group is Not defined!");
		if (audioPlayer.isPlaying) audioPlayer.Stop();
        audioPlayer.clip = null;
    }

    public void PopAudio(AudioClip sfxClip, Transform point, float volume = 0.5f, float pitch = 1.0f)
    {
        gameObject.SetActive(IsPlaying);
        transform.position = point.position;

        IsPlaying = true;

        audioPlayer.pitch = pitch;
        audioPlayer.PlayOneShot(sfxClip, volume);
        
        clipLength = sfxClip.length;
        Invoke(nameof(PushAudio), clipLength);
    }

    public void PushAudio()
    {
        IsPlaying = false;
        if(audioPlayer.isPlaying && audioPlayer.clip != null)
        {
            audioPlayer.Stop();
            audioPlayer.clip = null;
        }
        transform.position = Vector3.zero;
        audioPlayer.clip = null;
        clipLength = 0;
        gameObject.SetActive(IsPlaying);
    }
}
