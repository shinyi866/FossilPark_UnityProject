using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroControler : MonoBehaviour
{
    public Text text;
    private Gyroscope gyroscope;
    private Quaternion rot;
    private bool gyroEnable;
    private GameObject CameraContainer;
    
    public void Start()
    {
        CameraContainer = new GameObject("Camera Container");
        CameraContainer.transform.position = transform.position;
        transform.SetParent(CameraContainer.transform);
        gyroEnable = EnableGyro();
    }
    /*
    public void StartGyro()
    {
        CameraContainer = new GameObject("Camera Container");
        CameraContainer.transform.position = transform.position;
        transform.SetParent(CameraContainer.transform);
        gyroEnable = EnableGyro();
    }
    */
    private bool EnableGyro()
    {
        /*
        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
            transform.rotation = Quaternion.Euler(90, 0, 0); // 90,90,0
            //CameraContainer.transform.rotation = Quaternion.Euler(90, 90, 0);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }*/

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            transform.parent.Rotate(new Vector3(90f, 0f, 0f));

            return true;
        }

        return false;
    }

    void Update()
    {
        /*
        if (gyroEnable)
        {
            transform.localRotation = gyroscope.attitude * rot;
        }
        */
        if (gyroEnable)
        {
            // Invert the z and w of the gyro attitude
            this.transform.localRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            text.text = "this.transform.localRotation" + this.transform.localRotation;
        }
    }
}
