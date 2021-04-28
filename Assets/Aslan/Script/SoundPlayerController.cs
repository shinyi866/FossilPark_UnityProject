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
        bgmAudio.clip = soundData.SoundItems.bgm;
        bgmAudio.Play();
    }

    public void AlertSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.alertMission;
        bgmAudio.Play();
    }

    public void MatchSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.bonematch;
        bgmAudio.Play();
    }

    public void EnterSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.enterMission;
        soundEffectAudio.Play();
    }

    public void ErrorSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.error;
        bgmAudio.Play();
    }

    public void FinishSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.fnish;
        bgmAudio.Play();
    }

    public void FInishAllSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.finishAll;
        bgmAudio.Play();
    }

    public void NextSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.next;
        bgmAudio.Play();
    }

    public void TakePictureSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.takePicture;
        bgmAudio.Play();
    }
}
