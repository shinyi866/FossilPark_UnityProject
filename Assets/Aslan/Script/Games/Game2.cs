using UnityEngine;
using View;

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

        public bool isARGameStart;
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
            else
            {
                if(_camera.transform.position.x < -1.65)
                {
                    _camera.transform.position = new Vector3(-1.65f, _camera.transform.position.y, _camera.transform.position.z);
                }

                if (_camera.transform.position.x > 1.65)
                {
                    _camera.transform.position = new Vector3(1.65f, _camera.transform.position.y, _camera.transform.position.z);
                }

                if (_camera.transform.position.y > -1)
                {
                    _camera.transform.position = new Vector3(_camera.transform.position.x, -1, _camera.transform.position.z);
                }

                if (_camera.transform.position.z > 5)
                {
                    _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 5);
                }

                if (_camera.transform.position.z < 0)
                {
                    _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);
                }
            }
        }
    }
}
