using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : Singleton<Compass>
{
    private bool startCompass;
    private Camera _camera;

    void Start()
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

        transform.eulerAngles = new Vector3(0, (_camera.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading), 0);
        //text.text = $"_camera.transform.rotation: {_camera.transform.rotation}, headingAccuracy: {Input.compass.magneticHeading}";
    }
}
