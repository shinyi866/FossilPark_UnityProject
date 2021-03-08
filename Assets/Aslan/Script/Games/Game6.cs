using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Game6 : Game
    {
        //public GameObject target;
        public System.Action<bool> gameOverEvent;

        private Camera _camera;
        private PictureModal modal;
        private string videoPath = "Video/ele.mp4";
        private bool isGameStart;
        private int missionIndex = 6;
        private int successTimes;
        //private int failTimes;
        //private int times = 3;

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
                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 3))
                {
                    var cube = hit.transform;
                    var tag = hit.transform.gameObject.tag;

                    modal.TakePicture();

                    if (tag == "Cube1")
                    {
                        cube.position = cube.position + new Vector3(-2, 0, 0);
                        modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch1);

                    }
                    if (tag == "Cube2")
                    {
                        cube.position = cube.position + new Vector3(2, 0, 0);
                        modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch2);
                    }

                    successTimes++;
                }
                else if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 5))
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.HasCatch);
                }
                else
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.FailCatch);
                }
            });
        }

        private void Update()
        {
            if (!isGameStart) return;

            //TODO: update cube position
            //trackAnimal[0].transform.position = _camera.transform.position + videoSphere.transform.right * 150;
            //trackAnimal[0].transform.rotation = videoSphere.transform.rotation;

            if (successTimes == 2)
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
