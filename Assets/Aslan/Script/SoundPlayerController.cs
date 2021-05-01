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
        soundEffectAudio.loop = false;
    }

    public void MatchSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.bonematch;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void EnterSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.enterMission;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void ErrorSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.error;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void FinishSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.fnish;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void FInishAllSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.finishAll;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void NextSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.next;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void TakePictureSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.takePicture;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void BrushSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.brush;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void PenSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.pen;
        soundEffectAudio.Play();
        soundEffectAudio.loop = false;
    }

    public void RiverSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.river;
        soundEffectAudio.Play();
        soundEffectAudio.loop = true;
    }

    public void StopSoundEffect()
    {
        soundEffectAudio.clip = null;
    }
}
