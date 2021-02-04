using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ARcamera;

    private Camera currentCamera;

    private static CameraCtrl _instance;

    public static CameraCtrl instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraCtrl>();
                _instance.SetUp();
            }

            return _instance;
        }
    }

    public void SetUp()
    {
        MainCamera.transform.position = new Vector3(0, 0, 0);
        MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);

        ARcamera.transform.position = new Vector3(0, 0, 0);
        ARcamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Awake()
    {
        currentCamera = MainCamera.GetComponent<Camera>();

        if (ARcamera == null) Debug.Log("AR camera is null");
    }

    public void SwitchCamera(bool isAR)
    {
        var isARsupport = MainApp.Instance.isARsupport;
        currentCamera = MainCamera.GetComponent<Camera>();
        SetUp();

        if (!isARsupport) return;

        MainCamera.SetActive(!isAR);
        ARcamera.SetActive(isAR);

        currentCamera = isAR? ARcamera.GetComponent<Camera>() : MainCamera.GetComponent<Camera>();
        MediaPlayerController.instance.OpenMeshRender(!isAR);
    }

    public Camera GetCurrentCamera()
    {
        if (currentCamera == null) return null;

        return currentCamera;
    }
}
