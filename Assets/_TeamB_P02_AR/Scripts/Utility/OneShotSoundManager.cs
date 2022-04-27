using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSoundManager : SingletonMB<OneShotSoundManager>
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
    }

    public void PlaySound(AudioClip newSound, float volume)
    {
        if (newSound == null) return; //guard clause

        //setup
        _audioSource.clip = newSound;
        _audioSource.volume = volume;

        //play
        _audioSource.Play();
    }

}


