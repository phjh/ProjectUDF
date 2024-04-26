using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSource : MonoBehaviour
{
    public AudioSource audioPlayer;
    public bool IsPlaying {  get; private set; }
    private float clipLength = 0f;

    public void InitObject()
    {
        if(audioPlayer == null) audioPlayer = GetComponent<AudioSource>();
        audioPlayer.playOnAwake = false;
        audioPlayer.loop = false;

        if(audioPlayer.isPlaying) audioPlayer.Stop();
        audioPlayer.clip = null;
    }

    public void PopAudio(AudioClip sfxClip, Transform point, float volume = 0.5f, float pitch = 1.0f)
    {
        IsPlaying = true;

        gameObject.SetActive(IsPlaying);
        transform.position = point.position;

        audioPlayer.pitch = pitch;
        audioPlayer.PlayOneShot(sfxClip, volume);
        
        clipLength = sfxClip.length;
        Invoke(nameof(PushAudio), clipLength);
    }

    public void PushAudio()
    {
        IsPlaying = false;

        transform.position = Vector3.zero;
        clipLength = 0;
        audioPlayer.clip = null;
        gameObject.SetActive(IsPlaying);
    }
}
