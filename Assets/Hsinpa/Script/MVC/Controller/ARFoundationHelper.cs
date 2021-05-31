using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Hsinpa.Ctrl
{
    public class ARFoundationHelper : MonoBehaviour
    {
        private ARSession _arSession;
        public ARSession arSession => _arSession;

        private ARPlaneManager _arPlaneManager;
        public ARPlaneManager arPlaneManager => _arPlaneManager;

        private Camera _arCamera;
        public Camera arCamera => _arCamera;

        private ARRaycastManager _arRaycast;
        public ARRaycastManager arRaycast => _arRaycast;


        //private ARPoseDriver _arPoseDriver;
        //public ARPoseDriver arPoseDriver {
        //    get {
        //        if (_arPoseDriver == null)
        //            _arPoseDriver = _arCamera.GetComponent<ARPoseDriver>();

        //        return _arPoseDriver;
        //    }
        //}

        private List<ARPlane> arplanes = new List<ARPlane>();

        public void Awake()
        {
            _arSession = GameObject.FindObjectOfType<ARSession>();
            _arPlaneManager = GameObject.FindObjectOfType<ARPlaneManager>();

            var aRCameraManager = GameObject.FindObjectOfType<ARCameraManager>();

            if (aRCameraManager == null)
                _arCamera = CameraCtrl.instance.GetCurrentCamera();
            else
                _arCamera = aRCameraManager.GetComponent<Camera>();

            _arRaycast = _arPlaneManager.GetComponent<ARRaycastManager>();

            if (_arPlaneManager != null)
                _arPlaneManager.planesChanged += OnARPlaneChange;
        }

        private void OnARPlaneChange(ARPlanesChangedEventArgs p_arPlanesChangedEventArgs)
        {
            if (p_arPlanesChangedEventArgs.added != null)
                arplanes.AddRange(p_arPlanesChangedEventArgs.added);
        }

        public void ActivateAR(bool activate) {
            //arPoseDriver.enabled = activate;
            if (_arCamera != null)
                _arCamera.enabled = activate;

            if (_arSession != null)
                _arSession.enabled = activate;
        }

        public void ActivateFullAR(bool activate) {
            ActivateAR(activate);
            AcitvateARPlane(activate);
        }

        public void AcitvateARPlane(bool p_active) {
            try
            {
                foreach (var plane in arplanes)
                {
                    if (plane != null && plane.gameObject != null)
                        plane.gameObject.SetActive(p_active);
                }
            } catch
            {

            }

            if (_arPlaneManager)
                _arPlaneManager.enabled = p_active;
        }

        public void SetARCameraPos(Vector3 position, Quaternion quaternion)
        {
            if (_arCamera == null) return; 

            _arCamera.transform.rotation = quaternion;
            _arCamera.transform.position = position;
        }

    }
}