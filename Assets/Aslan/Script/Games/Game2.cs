using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameMission
{
    public class Game2 : Game
    {
        public System.Action<bool> gameOverEvent;

        private int missionIndex = 2;
        
        public Camera _camera;
        private bool isARGameStart;
        private bool isUnARGameStart;
        private int successTimes;
        private PictureModal modal;

        public void Init()
        {
            //_camera = CameraCtrl.instance.GetCurrentCamera();
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
                //UnsupportAR();
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
                modal.ConfirmButton.onClick.AddListener(() =>
                {
                    isUnARGameStart = false;
                    GameResult(true);
                });
            }

        }
    }
}
