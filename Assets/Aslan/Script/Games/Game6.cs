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
        //private PictureModal pictureModal;
        private string videoPath = "Video/ele.mp4";
        private bool isGameStart;
        private int missionIndex = 6;
        private int successTimes;
        //private int failTimes;
        //private int times = 3;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();

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
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game6);
            //pictureModal.ShowInfo(missionIndex, TypeFlag.PictureType.MissionType);
            MediaPlayerController.instance.PlayVideo();            
            RaycastHit hit;

            modal.game6Panel.button.onClick.AddListener(() =>
            {
                Debug.Log("take picture");
                modal.TakePicture();
                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 3))
                {
                    var cube = hit.transform;
                    var tag = hit.transform.gameObject.tag;

                    //===pictureModal.TakePicture();
                    modal.TakePicture();
                    Debug.Log("take picture1");
                    if (tag == "Cube1")
                    {
                        cube.position = cube.position + new Vector3(-2, 0, 0);
                        modal.ShowModal(6, TypeFlag.ARGameType.GamePrompt);
                        //===pictureModal.ShowInfo(missionIndex, TypeFlag.PictureType.Result1);

                    }
                    if (tag == "Cube2")
                    {
                        cube.position = cube.position + new Vector3(2, 0, 0);
                        //===pictureModal.ShowInfo(missionIndex, TypeFlag.PictureType.Result2);
                    }

                    successTimes++;
                }
                else if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 5))
                {
                    //===pictureModal.TakePicture();
                    //===pictureModal.ShowInfo(missionIndex, TypeFlag.PictureType.HasCatch);
                }
                else
                {
                    //===pictureModal.TakePicture();
                    //===pictureModal.ShowInfo(missionIndex, TypeFlag.PictureType.FailCatch);
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
                //==pictureModal.PictureButton.interactable = false;
                //==pictureModal.ConfirmButton.onClick.AddListener(() =>
                //=={
                    isGameStart = false;
                    GameResult(true);
                //==});
            }

        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }
    }
}
