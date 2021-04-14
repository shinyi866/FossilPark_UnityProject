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
        private GameDialogData data;
        private ARGameModal modal;
        private DialogModal dialogmodel;
        private string videoPath = "AVProVideoSamples/BigBuckBunny_720p30.mp4";

        public void Init()
        {
            GameModals.instance.OpenAR();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.OpenModal<ARGameModal>();
            data = MainApp.Instance.database;
            modal.text.text = data.m_Data[missionIndex].gameNotify[0];
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
            modal.SwitchConfirmButton(true);
            modal.text.text = data.m_Data[missionIndex].gamePrompt[0];
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
            GameModals.instance.CloseModal();

            yield return new WaitForSeconds(5f);

            var dialogmodel = GameModals.instance.OpenModal<DialogModal>();
            dialogmodel.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);
            dialogmodel.ConfirmButton.onClick.AddListener(() =>
            {
                GameModals.instance.CloseModal();
                isGameStart = false;
                GameResult();
            });
            
            isARanimationEnd = false;

        }
    }
}