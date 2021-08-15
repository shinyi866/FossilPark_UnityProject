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
    [SerializeField]
    private AudioSource soundEffectAudio2;

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
        soundEffectAudio2.clip = soundData.SoundItems.fnish;
        soundEffectAudio2.Play();
        soundEffectAudio2.loop = false;
    }

    public void FinishAllSoundEffect()
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
        if(!soundEffectAudio.isPlaying)
        {
            soundEffectAudio.clip = soundData.SoundItems.brush;
            soundEffectAudio.Play();
            soundEffectAudio.loop = false;
        }
        
    }

    public void PenSoundEffect()
    {
        if (!soundEffectAudio.isPlaying)
        {
            soundEffectAudio.clip = soundData.SoundItems.pen;
            soundEffectAudio.Play();
            soundEffectAudio.loop = false;
        }       
    }

    public void RiverSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.river;
        soundEffectAudio.Play();
        soundEffectAudio.loop = true;
    }

    public void TRexSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.TRex;
        soundEffectAudio.Play();
        soundEffectAudio.loop = true;
    }

    public void BrachiosaurusSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.Brachiosaurus;
        soundEffectAudio.Play();
        soundEffectAudio.loop = true;
    }

    public void TriceratopSoundEffect()
    {
        soundEffectAudio.clip = soundData.SoundItems.Triceratop;
        soundEffectAudio.Play();
        soundEffectAudio.loop = true;
    }

    public void StopSoundEffect()
    {
        if (soundEffectAudio.clip != null)
            soundEffectAudio.clip = null;
    }
}
