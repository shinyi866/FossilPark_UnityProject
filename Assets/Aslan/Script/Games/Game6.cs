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

        [SerializeField]
        private Transform cube1;
        [SerializeField]
        private Transform cube2;
        private Camera _camera;
        private ARGameModal modal;
        private string videoPath = "Video/ele.mp4";
        //private bool isGameStart;
        private int missionIndex = 6;
        private int successTimes = 0;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            cube1.position = new Vector3(2.8f, 0, 3.4f);
            cube2.position = new Vector3(-5.6f, 0, 1.4f);
            successTimes = 0;

            Modals.instance.CloseAllModal();
            MediaPlayerController.instance.LoadVideo(videoPath);
        }

        public void GameStart()
        {
            isGameStart = true;
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game6);
            modal.game6Panel.button.onClick.RemoveAllListeners();
            RaycastHit hit;

            modal.game6Panel.button.onClick.AddListener(() =>
            {
                modal.TakePicture();

                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 6))
                {
                    var cube = hit.transform;
                    var tag = hit.transform.gameObject.tag;

                    if (tag == "Cube1" && successTimes == 0)
                    {
                        cube.position = cube.position + new Vector3(-2, 0, 0);
                        modal.ShowPrompt(6, TypeFlag.ARGameType.GamePrompt1);
                        modal.text.text = MainApp.Instance.database.m_Data[missionIndex].gameNotify[1];
                        successTimes++;
                    }
                    else if (tag == "Cube2" && successTimes == 1)
                    {
                        cube.position = cube.position + new Vector3(2, 0, 0);
                        modal.ShowPrompt(6, TypeFlag.ARGameType.GamePrompt2);
                        successTimes++;
                    }
                    else
                    {
                        modal.ShowPrompt(6, TypeFlag.ARGameType.GamePrompt4);
                    }
                }
                else if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 7))
                {
                    modal.ShowPrompt(6, TypeFlag.ARGameType.GamePrompt3);
                }
                else
                {
                    modal.ShowPrompt(6, TypeFlag.ARGameType.GamePrompt4);
                }
                
                
            });
        }

        private void Update()
        {
            if (!isGameStart) return;
            
            if (successTimes == 2)
            {
                modal.SwitchConfirmButton(true);
                modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
                {
                    isGameStart = false;
                    modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
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
