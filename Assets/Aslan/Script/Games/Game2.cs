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
        
        private Camera _camera;
        private bool isARGameStart;
        private bool isUnARGameStart;
        //private bool _isARsupport;
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

        public void GameStart(bool isARsupport)
        {
            if (isARsupport)
            {
                isARGameStart = true;
                SupportAR();
            }
            else
            {
                isUnARGameStart = true;
                UnsupportAR();
            }

        }

        private void SupportAR()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private void UnsupportAR()
        {
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
            if (isARGameStart)
            {
                isARGameStart = false;
                Debug.Log("AR Game");
            }

            if (isUnARGameStart && successTimes == 3)
            {
                modal.PictureButton.interactable = false;
                modal.ConfirmButton.onClick.AddListener(() =>
                {
                    isUnARGameStart = false;
                    GameResult(true);
                });
            }

        }
    }
}
