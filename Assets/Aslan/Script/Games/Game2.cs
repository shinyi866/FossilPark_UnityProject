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
        private GameObject UIobject;
        [SerializeField]
        private GameObject movieObject;
        [SerializeField]
        private bool TestMode;

        private int missionIndex = 2;

        private bool isARGameStart;
        private bool isUnARGameStart;
        private bool isGameStart;
        private float time = 2;
        private Camera _camera;
        private ARGameModal modal;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            isGameStart = true;
            movieObject.SetActive(false);
        }

        public void GameStart(bool isARsupport)
        {
            UIobject.SetActive(true);
            SoundPlayerController.Instance.RiverSoundEffect();
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            if (isARsupport)
            {
                isARGameStart = true;
            }
            else
            {
                isUnARGameStart = true;                
                UnsupportAR();
            }

        }

        private void ResetDirection()
        {
            if(time > 0)
            {
                Compass.Instance.SetUp(Object, 2);
                time -= Time.deltaTime;
            }
        }

        private void UnsupportAR()
        {
            Debug.Log("unsupport AR");
            movieObject.SetActive(true);
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            var _cameraFront = _camera.transform.forward;
            _cameraFront.y = -1.5f;
            _cameraFront.z = 3f;

            riverObject.transform.position = _camera.transform.position + _cameraFront;
            riverObject.transform.eulerAngles = new Vector3(0, 90, 0);

            _camera.transform.eulerAngles = new Vector3(40, 0, 0);
        }
        
        private void Update()
        {
            if (!isGameStart) return;

            if(isARGameStart)
                ResetDirection();
        }
    }
}
