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
        //private int times = 5;
        private int successTimes;
        //private int failTimes;
        private PictureModal modal;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.GetModal<PictureModal>();

            Modals.instance.CloseAllModal();
        }

        public void GameStart()
        {
            isGameStart = true;

            if (MainApp.Instance.isARsupport)
            {
                // TODO: wait Joy
            }
            else
            {
                UnsupportAR();
            }
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private void UnsupportAR()
        {
            
            MediaPlayerController.instance.LoadVideo(videoPath);
            MediaPlayerController.instance.PlayVideo();
            RaycastHit hit;

            modal.PictureButton.onClick.AddListener(() =>
            {
                //times--;

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
                        cube.position = cube.position + new Vector3(0, 0, -2);
                        modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch2);
                    }
                    if (tag == "Cube3")
                    {
                        cube.position = cube.position + new Vector3(2, 0, 0);
                        modal.ShowInfo(missionIndex, TypeFlag.PictureType.SuccessCatch3);
                    }
                    
                    successTimes++;
                }
                else if (!Physics.Raycast(transform.position, _camera.transform.forward, 3) && Physics.Raycast(transform.position, _camera.transform.forward, 5))
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.HasCatch);
                    //failTimes++;
                }
                else
                {
                    modal.TakePicture();
                    modal.ShowInfo(missionIndex, TypeFlag.PictureType.FailCatch);
                    //failTimes++;
                }
            });
        }

        private void Update()
        {
            if (!isGameStart) return;

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
