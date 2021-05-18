using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraCtrl : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ARcamera;

    private Camera currentCamera;
    private AROcclusionManager occlusionManager;

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
        occlusionManager = ARcamera.GetComponent<AROcclusionManager>();

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
        MediaPlayerController.instance.OpenSphereVideo(!isAR);
    }

    public void SwitchCameraForHsinpaMission(bool isAR)
    {
        currentCamera = MainCamera.GetComponent<Camera>();
        SetUp();

        MainCamera.SetActive(!isAR);
        ARcamera.SetActive(isAR);

        currentCamera = isAR ? ARcamera.GetComponent<Camera>() : MainCamera.GetComponent<Camera>();
        MediaPlayerController.instance.OpenSphereVideo(!isAR);
    }

    public Camera GetCurrentCamera()
    {
        if (currentCamera == null) return null;

        return currentCamera;
    }

    public void OcclusionForEnviroment()
    {
        var isARsupport = MainApp.Instance.isARsupport;
        if (!isARsupport) return;
        Debug.Log("OcclusionForEnviroment");
        occlusionManager.enabled = true;
        occlusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Medium;
        occlusionManager.requestedHumanDepthMode = UnityEngine.XR.ARSubsystems.HumanSegmentationDepthMode.Disabled;
        occlusionManager.requestedHumanStencilMode = UnityEngine.XR.ARSubsystems.HumanSegmentationStencilMode.Disabled;
        occlusionManager.requestedOcclusionPreferenceMode = UnityEngine.XR.ARSubsystems.OcclusionPreferenceMode.PreferEnvironmentOcclusion;
    }

    public void OcclusionForHuman()
    {
        var isARsupport = MainApp.Instance.isARsupport;
        if (!isARsupport) return;
        Debug.Log("OcclusionForHuman");
        occlusionManager.enabled = true;
        occlusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Disabled;
        occlusionManager.requestedHumanDepthMode = UnityEngine.XR.ARSubsystems.HumanSegmentationDepthMode.Fastest;
        occlusionManager.requestedHumanStencilMode = UnityEngine.XR.ARSubsystems.HumanSegmentationStencilMode.Medium;
        occlusionManager.requestedOcclusionPreferenceMode = UnityEngine.XR.ARSubsystems.OcclusionPreferenceMode.PreferHumanOcclusion;
    }

    public void DisableOcclusionManager()
    {
        occlusionManager.enabled = false;
    }
}
