using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerController : MonoBehaviour
{
    [SerializeField]
    private MediaPlayer _mediaPlayer = null;

    [SerializeField]
    private MeshRenderer meshRenderer;

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
        _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, filePath, true);
    }

    public void LoadVideo(string filePath)
    {
        _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, filePath, false);
        _mediaPlayer.m_Loop = true;
    }

    public void PlayVideo()
    {
        _mediaPlayer.Control.Play();
    }

    public void CloseVideo()
    {
        _mediaPlayer.Control.CloseVideo();
    }

    public void OpenVideo()
    {
        //_mediaPlayer.VideoOpened();
    }

    public void OpenMeshRender(bool isOpen)
    {
        meshRenderer.enabled = isOpen;
    }
}
