using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Game2 : Game
    {
        [SerializeField]
        private GameObject[] trackAnimal;

        public System.Action<bool> gameOverEvent;

        private int missionIndex = 2;
        private string videoPath = "Video/4096.mp4";
        private Camera _camera;
        private bool isGameStart;
        private int times = 5;
        private int successTimes;
        private int failTimes;
        private PictureModal modal;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.GetModal<PictureModal>();

            for (int i = 0; i < trackAnimal.Length; i++)
            {
                var addPosition = 90;
                //TODO: set cube position

                //Object.transform.position = _camera.transform.position + videoSphere.transform.right * addPosition * i;
                //Object.transform.rotation = videoSphere.transform.rotation;
            }

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

                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 3))
                {
                    var cube = hit.transform;
                    var tag = hit.transform.gameObject.tag;

                    if (tag == "Cube1") { cube.position = cube.position + new Vector3(-2, 0, 0); }
                    if (tag == "Cube2") { cube.position = cube.position + new Vector3(0, 0, -2); }
                    if (tag == "Cube3") { cube.position = cube.position + new Vector3(2, 0, 0); }

                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch);
                    successTimes++;
                }
                else if (!Physics.Raycast(transform.position, _camera.transform.forward, 3) && Physics.Raycast(transform.position, _camera.transform.forward, 5))
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.HasCatch);
                    failTimes++;
                }
                else
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.FailCatch);
                    failTimes++;
                }
            });
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
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
                    bool isSuccess = successTimes == 3;
                    isGameStart = false;
                    modal.ResetView();
                    GameResult(isSuccess);
                });
            }

            if (successTimes == 3)
            {
                modal.PictureButton.interactable = false;
                modal.ConfirmButton.onClick.AddListener(() =>
                {
                    isGameStart = false;
                    GameResult(true);
                });
            }

        }
    }
}
