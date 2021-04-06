using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Game1 : Game
    {
        [SerializeField]
        private GameObject ARObject;
        public System.Action gameOverEvent;

        private int missionIndex = 1;
        private Camera _camera;
        private bool isGameStart;
        private bool isVideoEnd;
        private bool isARanimationEnd; // wait real animation, use animation end etect
        private ARGameModal modal;
        private string videoPath = "AVProVideoSamples/BigBuckBunny_720p30.mp4";

        public void Init()
        {
            GameModals.instance.OpenAR();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            ARObject.SetActive(false);
        }

        public void GameStart()
        {
            isGameStart = true;
            MediaPlayerController.instance.LoadAndPlay2DVideoNotLoop(videoPath);
        }

        private void GameResult()
        {
            if (gameOverEvent != null)
                gameOverEvent();
        }

        private void Update()
        {
            if (!isGameStart) return;

            if (MediaPlayerController.instance.isVideoFinish() && !isVideoEnd)
            {
                Debug.Log("video end");
                ShowARObject();
                isVideoEnd = true;
            }

            if(isARanimationEnd)
            {
                StartCoroutine(CheckARSupport());
            }
        }

        private void ShowARObject()
        {
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.SwitchConfirmButton(true);
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt1);            
            modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
            {
                MediaPlayerController.instance.Destroy2DPlane();
                modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
                modal.SwitchConfirmButton(false);
                ARObject.SetActive(true);

                isARanimationEnd = true;
            });            
        }

        private IEnumerator CheckARSupport()
        {
            yield return new WaitForSeconds(20f);

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                isGameStart = false;
                GameResult();
            });
            
            isARanimationEnd = false;

        }
    }
}