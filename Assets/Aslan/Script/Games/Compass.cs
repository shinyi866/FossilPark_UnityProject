using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : Singleton<Compass>
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Camera _camera;

    private bool startCompass;

    void Start()
    {
        //_camera = CameraCtrl.instance.GetCurrentCamera(); 
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
        if (_camera.gameObject.activeInHierarchy == false) return;

        transform.eulerAngles = new Vector3(0, (_camera.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading), 0);
        var r = Quaternion.Euler(0, 0, (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading);
        text.text = $"transform.eulerAngles: {transform.eulerAngles}, _camera.transform.rotation: {_camera.transform.rotation.eulerAngles.y}, r: {r}, CurrentHeading.heading: {(float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading}";
    }
}
