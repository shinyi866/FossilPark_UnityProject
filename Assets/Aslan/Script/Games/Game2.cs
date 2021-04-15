using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Game2 : Game
    {
        [SerializeField]
        private GameObject riverObject;
        [SerializeField]
        private ARPlaneManager planeManager;
        [SerializeField]
        private GameObject UIobject;
        [SerializeField]
        private bool TestMode;

        public System.Action<bool> gameOverEvent;

        private int missionIndex = 2;

        private bool isARGameStart;
        private bool isUnARGameStart;
        private int successTimes;
        private Camera _camera;
        private GameObject placeObject;
        private ARGameModal modal;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            /*
            placeObject = Instantiate(riverObject);
            var frontPos = _camera.transform.forward * 3;
            placeObject.transform.SetParent(this.transform);
            placeObject.transform.position = new Vector3(riverObject.transform.position.x, -3, frontPos.z);
            */
            if (!TestMode) return;

            //riverObject.SetActive(false);
        }        

        public void GameStart(bool isARsupport)
        {
            UIobject.SetActive(true);

            if (isARsupport)
            {
                isARGameStart = true;
                SupportAR();
            }
            else
            {
                isUnARGameStart = true;
                //UnsupportAR();
            }

        }

        private void SupportAR()
        {
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            planeManager.enabled = true;
            planeManager.planesChanged += PlaneChange;
        }
        
        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        // AR Plane Track
        private void PlaneChange(ARPlanesChangedEventArgs args)
        {
            if (args.added != null && placeObject == null)
            {
                ARPlane aRPlane = args.added[0];

                placeObject = Instantiate(riverObject, aRPlane.transform.position, Quaternion.identity);
                //placeObject.transform.position = new Vector3(placeObject.transform.position.x, aRPlane.transform.position.y, placeObject.transform.position.z);
                //riverObject.SetActive(true);
                //riverObject.transform.position = new Vector3(riverObject.transform.position.x, aRPlane.transform.position.y - 3, riverObject.transform.position.z);
                Debug.Log("======placeObject " + placeObject.transform.position);
                Debug.Log("======aRPlane " + aRPlane.transform.position);
            }
            else
            {
                modal.text.text = "請掃描周遭地面";
            }
        }

        private void UnsupportAR()
        {
            planeManager.enabled = false;
            planeManager.planesChanged -= PlaneChange;
            Debug.Log("unsupport AR");
        }
        
        private void Update()
        {
            if (isARGameStart)
            {
                isARGameStart = false;
                Debug.Log("AR Game");
            }

            if (isUnARGameStart && successTimes == 3)
            {
                //===modal.PictureButton.interactable = false;
                //modal.ConfirmButton.onClick.AddListener(() =>
                //{
                    isUnARGameStart = false;
                    GameResult(true);
                //});
            }

        }
    }
}
