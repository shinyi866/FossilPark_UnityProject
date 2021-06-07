﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerController : MonoBehaviour
{
    [SerializeField]
    private MediaPlayer _mediaPlayer = null;
    [SerializeField]
    private GameObject sphereVideo;
    [SerializeField]
    private GameObject planeVideo;

    private static MediaPlayerController _instance;

    public static MediaPlayerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MediaPlayerController>();
            }

            return _instance;
        }
    }

    public void LoadAndPlayVideo(string filePath)
    {
        OpenSphereVideo(true);
        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = true;
    }

    public void LoadAndPlayVideoNotLoop(string filePath)
    {
        OpenSphereVideo(true);
        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = false;
    }

    public void LoadAndPlay2DVideoNotLoop(string filePath)
    {        
        SwitchTo2DPlane(true);
        OpenSphereVideo(false);

        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = false;
    }

    public void LoadVideo(string filePath)
    {
        OpenSphereVideo(true);
        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, false);
        _mediaPlayer.Loop = false;
    }

    public void PlayVideo()
    {
        _mediaPlayer.Play();
    }

    public void StopVideo()
    {
        _mediaPlayer.Control.CloseMedia();
    }

    public void CloseVideo()
    {
        _mediaPlayer.Control.CloseMedia();
        OpenSphereVideo(false);
    }

    //public void OpenVideo()
    //{
        //_mediaPlayer.VideoOpened();
    //}

    public void OpenSphereVideo(bool isOpen)
    {
        sphereVideo.SetActive(isOpen);
        //meshRenderer.enabled = isOpen;
    }

    public bool isVideoFinish()
    {
        return _mediaPlayer.Control.IsFinished();
    }
    
    public void SwitchTo2DPlane(bool isOpen)
    {
        sphereVideo.SetActive(!isOpen);
        planeVideo.SetActive(isOpen);
    }
    
    public void Close2DPlane()
    {
        planeVideo.SetActive(false);
    }
}
