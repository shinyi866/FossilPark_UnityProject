using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : Singleton<Compass>
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text1;
    [SerializeField]
    private Text text2;

    private Camera _camera;

    private bool startCompass;

    public void SetUp(GameObject gameObject, float rotationOffset)
    {
        _camera = CameraCtrl.instance.GetCurrentCamera();
        
        var faceDir = this.transform.rotation.eulerAngles;
        faceDir.y += rotationOffset;
        gameObject.transform.position = _camera.transform.position;
        gameObject.transform.rotation = Quaternion.Euler(faceDir);

        text2.text = $"set up: {_camera.transform.position}, gameObject: {gameObject.transform.position}, current camera: {_camera.transform.position}";
    }

    private void Start()
    {
        _camera = CameraCtrl.instance.GetCurrentCamera();
        InitializeCompass();
    }

    private void InitializeCompass()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(InitializeCheck());
    }

    private IEnumerator InitializeCheck()
    {
        yield return new WaitForSeconds(1f);
        startCompass = Input.compass.enabled;
    }

    void Update()
    {
        if (!startCompass) return;
        if (!_camera.gameObject.activeSelf) return;

        transform.eulerAngles = new Vector3(0, (_camera.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading), 0);
        var r = Quaternion.Euler(0, 0, (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading);
        text.text = $"transform.eulerAngles: {transform.eulerAngles}, _camera.transform.rotation: {_camera.transform.rotation.eulerAngles.y}, r: {r}, CurrentHeading.heading: {(float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading}";
        text1.text = $"this pos: {this.transform.position}, current camera pos: {_camera.transform.position}"; 
    }
}
