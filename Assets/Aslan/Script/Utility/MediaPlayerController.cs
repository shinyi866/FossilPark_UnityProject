using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerController : MonoBehaviour
{
    //[SerializeField]
    private MediaPlayer _mediaPlayer = null;
    //[SerializeField]
    //[SerializeField]
    private GameObject video2D;
    private GameObject video3D;

    public GameObject videoObject;
    private GameObject currentVideo;

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

    public void SetUp()
    {
        currentVideo = Instantiate(videoObject);
        video3D = currentVideo.transform.GetChild(0).gameObject;
        video2D = currentVideo.transform.GetChild(1).gameObject;
        _mediaPlayer = currentVideo.transform.GetChild(2).gameObject.GetComponent<MediaPlayer>();
    }

    public void LoadAndPlayVideo(string filePath)
    {
        SwitchToVideo360(true);
        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = true;
    }

    public void LoadAndPlayVideoNotLoop(string filePath)
    {
        SwitchToVideo360(true);
        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = false;
    }

    public void LoadAndPlay2DVideoNotLoop(string filePath)
    {        
        SwitchToVideo360(false);

        _mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, filePath, true);
        _mediaPlayer.Loop = false;
    }

    public void LoadVideo(string filePath)
    {
        SwitchToVideo360(true);
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

    public void DestroyVideo()
    {
        _mediaPlayer.Control.CloseMedia();

        if(currentVideo != null)
            Destroy(currentVideo);
        //SwitchToVideo360(false);
    }

    public void SwitchToVideo360(bool isOpen)
    {
        video3D.SetActive(isOpen);
        video2D.SetActive(!isOpen);
    }

    public bool isVideoFinish()
    {        
        if(_mediaPlayer != null)
            return _mediaPlayer.Control.IsFinished();
        else
            return false;
    }
    
    public void CloseVideo()
    {
        video2D.SetActive(false);
        video3D.SetActive(false);
    }
}
