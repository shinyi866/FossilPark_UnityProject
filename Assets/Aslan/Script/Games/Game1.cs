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
        //[SerializeField]
        //private GameObject videoPlane;
        public System.Action gameOverEvent;

        private int missionIndex = 1;
        private float time = 2f;
        private bool isGameStart;
        private bool isARsupport;
        private bool isVideoEnd;
        private bool isSetVideoPlane;
        private bool isARanimationEnd; // wait real animation, use animation end etect
        private GameDialogData data;
        private ARGameModal modal;
        private Camera _camera;
        private string videoPath = "Video/animation.mp4";
        private string videoBGPath = "Video/entrance.mp4";

        public void Init()
        {
            Modals.instance.CloseAllModal();

            isARsupport = MainApp.Instance.isARsupport;
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.OpenModal<ARGameModal>();
            data = MainApp.Instance.database;
            modal.text.text = data.m_Data[missionIndex].gameNotify[0];
            ARObject.SetActive(false);
        }

        public void GameStart()
        {
            isGameStart = true;
            SoundPlayerController.Instance.PauseBackgroundMusic();
            MediaPlayerController.instance.LoadAndPlay2DVideoNotLoop(videoPath);
        }

        private void ResetDirection()
        {
            if (time > 0)
            {
                if (isARsupport)
                {
                    Compass.Instance.SetUp(Object, 96);
                    time -= Time.deltaTime;
                }
                else
                {
                    var faceDir = this.transform.rotation.eulerAngles;
                    faceDir.y += 180;
                    Object.transform.position = new Vector3(9,-1,5);
                    Object.transform.rotation = Quaternion.Euler(faceDir);
                }
            }
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
                if (!MainApp.Instance.isARsupport)
                {
                    MediaPlayerController.instance.LoadVideo(videoBGPath);
                    ARObject.transform.localScale = new Vector3(7, 7, 7);
                }                    

                ShowARObject();
                isVideoEnd = true;               
            }

            if(isARanimationEnd)
            {
                ResetDirection();
                StartCoroutine(CheckARSupport());
            }
        }

        private void ShowARObject()
        {
            GameModals.instance.OpenAR(); // Stop AR Camera rotate

            if (MainApp.Instance.isARsupport)
                MediaPlayerController.instance.CloseVideo();
            else
                MediaPlayerController.instance.SwitchToVideo360(true);

            SoundPlayerController.Instance.PlayBackgroundMusic();
            CameraCtrl.instance.OcclusionForHuman();

            modal.SwitchConfirmButton(true);
            modal.text.text = data.m_Data[missionIndex].gamePrompt[1];
            modal.gamePromptPanel.image.sprite = data.m_Data[missionIndex].endPicutre;
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt1);            
            modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
            {
                modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);
                modal.SwitchConfirmButton(false);
                ARObject.SetActive(true);

                isSetVideoPlane = false;
                isARanimationEnd = true;
            });            
        }

        private IEnumerator CheckARSupport()
        {
            GameModals.instance.CloseModal();

            yield return new WaitForSeconds(30f);

            var dialogmodel = GameModals.instance.OpenModal<DialogModal>();
            dialogmodel.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);
            dialogmodel.ConfirmButton.onClick.AddListener(() =>
            {
                GameModals.instance.CloseModal();
                Modals.instance.OpenModal<ARModal>();
                isGameStart = false;
                //GameResult();
            });
            
            isARanimationEnd = false;

            StopAllCoroutines();
        }
    }
}