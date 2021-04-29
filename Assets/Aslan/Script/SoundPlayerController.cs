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

    public void PauseBackgroundMusic()
    {
        bgmAudio.clip = null;
    }

    public void AlertSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.alertMission;
        soundEffectAudio.Play();
    }

    public void MatchSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.bonematch;
        soundEffectAudio.Play();
    }

    public void EnterSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.enterMission;
        soundEffectAudio.Play();
    }

    public void ErrorSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.error;
        soundEffectAudio.Play();
    }

    public void FinishSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.fnish;
        soundEffectAudio.Play();
    }

    public void FInishAllSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.finishAll;
        soundEffectAudio.Play();    
    }

    public void NextSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.next;
        soundEffectAudio.Play();
    }

    public void TakePictureSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.takePicture;
        soundEffectAudio.Play();
    }
}
