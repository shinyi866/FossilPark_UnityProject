﻿using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip bgm;
    public AudioClip alertMission;
    public AudioClip bonematch;
    public AudioClip enterMission;
    public AudioClip error;
    public AudioClip fnish;
    public AudioClip finishAll;
    public AudioClip next;
    public AudioClip takePicture;
    public AudioClip brush;
    public AudioClip pen;
    public AudioClip river;
    public AudioClip TRex;
    public AudioClip Brachiosaurus;
    public AudioClip Triceratop;
}

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 0)]
public class SoundData : ScriptableObject
{
    [SerializeField]
    public Sound SoundItems;

}
