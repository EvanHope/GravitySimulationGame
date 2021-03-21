using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> clips = new List<AudioClip>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int i)
    {
        audioSource.PlayOneShot(clips[i]);
    }
    public void StopAudio()
    {
        audioSource.Stop();
    }
}

