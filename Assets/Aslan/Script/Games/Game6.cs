using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Game6 : Game
    {
        public GameObject target;
        public System.Action<bool> gameOverEvent;

        private Camera _camera;
        private PictureModal modal;
        private string videoPath = "Video/ele.mp4";
        private bool isGameStart;
        private int missionIndex = 6;
        private int successTimes;
        private int failTimes;
        private int times = 3;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.GetModal<PictureModal>();

            var addPosition = 90;
            //TODO: set cube position

            //Object.transform.position = _camera.transform.position + videoSphere.transform.right * addPosition * i;
            //Object.transform.rotation = videoSphere.transform.rotation;

            Modals.instance.CloseAllModal();
            MediaPlayerController.instance.LoadVideo(videoPath);
        }

        public void GameStart()
        {
            isGameStart = true;
            MediaPlayerController.instance.PlayVideo();
            RaycastHit hit;

            modal.PictureButton.onClick.AddListener(() =>
            {
                times--;

                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 5))
                {
                    var cube = hit.transform;
                    var tag = hit.transform.gameObject.tag;

                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch);
                    successTimes++;
                }
                else
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.FailCatch);
                    failTimes++;
                }
            });
        }

        private void Update()
        {
            if (!isGameStart) return;

            //TODO: update cube position
            //trackAnimal[0].transform.position = _camera.transform.position + videoSphere.transform.right * 150;
            //trackAnimal[0].transform.rotation = videoSphere.transform.rotation;

            if (times == 0)
            {
                modal.PictureButton.interactable = false;
                modal.ConfirmButton.onClick.AddListener(() =>
                {
                    bool isSuccess = successTimes == 1;
                    isGameStart = false;
                    modal.ResetView();
                    GameResult(isSuccess);
                });
            }

            if (successTimes == 1)
            {
                modal.PictureButton.interactable = false;
                modal.ConfirmButton.onClick.AddListener(() =>
                {
                    isGameStart = false;
                    GameResult(true);
                });
            }

        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }
    }
}
