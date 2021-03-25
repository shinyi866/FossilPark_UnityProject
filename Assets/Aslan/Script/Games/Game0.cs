using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Game0 : Game
    {
        public GameObject ClockObject;

        private int missionIndex;
        private Camera _camera;
        private bool isGameStart;
        private bool placeClock;
        private ARGameModal modal;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game0);
            ClockObject.SetActive(false);

            modal.game0Panel.button.onClick.AddListener(()=> {
                placeClock = true;
                ClockObject.SetActive(true);
            });
        }

        public void GameStart()
        {
            isGameStart = true;
        }

        void Update()
        {
            if (!isGameStart) return;

            if(!placeClock)
            {
                var _cameraFront = _camera.transform.forward;
                var _frontPos = _cameraFront * 15;

                _cameraFront.y = 0;
                ClockObject.transform.position = _camera.transform.position + _frontPos;
                ClockObject.transform.rotation = Quaternion.LookRotation(_cameraFront);
            }
        }
    }
}

