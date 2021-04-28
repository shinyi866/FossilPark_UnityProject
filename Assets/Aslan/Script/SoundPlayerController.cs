using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerController : Singleton<SoundPlayerController>
{
    [SerializeField]
    private SoundData soundData;
    [SerializeField]
    private AudioSource bgmAudio;
    [SerializeField]
    private AudioSource soundEffectAudio;

    public void PlayBackgroundMusic()
    {
        bgmAudio.clip = soundData.SoundItems[0].bgm;
        bgmAudio.Play();
    }
}
