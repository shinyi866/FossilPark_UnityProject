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

        public bool isMisssionEnd;

        private int missionIndex = 1;
        private float time = 2f;
        //private bool isGameStart;
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

            time = 2;
            data = MainApp.Instance.database;
            isARsupport = MainApp.Instance.isARsupport;
            _camera = CameraCtrl.instance.GetCurrentCamera();
            modal = GameModals.instance.OpenModal<ARGameModal>();
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
            //CameraCtrl.instance.OcclusionForHuman();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game1);
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt1);
            modal.SwitchConfirmButton(true);
            modal.gamePromptPanel.button_confirm.onClick.AddListener(() =>
            {
                modal.ShowPanel(modal.gamePromptPanel.canvasGroup, false);                
                ARObject.SetActive(true);
                modal.CloseAllPanel();
                modal.SwitchConfirmButton(false);

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
                isMisssionEnd = true;
                //GameResult();
            });
            
            isARanimationEnd = false;

            StopAllCoroutines();
        }
    }
}