using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using Hsinpa.Ctrl;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Mission7 : Mission
    {
        public GameObject hisnpaPrefab;
        public CrocoBoneRepairCtrl crocoCtrl;
        //public ARPlaneManager planeManager;

        private int missionIndex = 7;
        private bool isEventOpen;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();

            //Ar support for hsinpa check
            if (!MainApp.Instance.isARsupport)
            {
                MediaPlayerController.instance.SetUp();
                MediaPlayerController.instance.LoadAndPlayVideo("Video/scence_360.mp4", true);
            }
            else
            {
                //CameraCtrl.instance.SwitchToARCamera(true);
                CameraCtrl.instance.OpenARPlaneManager(true);
            }

            hisnpaPrefab.SetActive(true);
        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);

            isEventOpen = true;
            crocoCtrl.OnEndGameEvent += EndGame;
            crocoCtrl.EnterGame(87, MainApp.Instance.isARsupport);
        }

        public void EndGame(bool isSuccess)
        {
            isEventOpen = false;
            crocoCtrl.OnEndGameEvent -= EndGame;

            var ARmodal = GameModals.instance.OpenModal<ARGameModal>();
            ARmodal.ShowPanel(ARmodal.gamePromptPanel.canvasGroup, false);

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);

            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                GameModals.instance.CloseModal();

                if (MainApp.Instance.isARsupport)
                    GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                else
                    GameModals.instance.GetBackAnimalNoAR(missionIndex);
            });
        }

        public void BackToMain()
        {
            if(isEventOpen)
            {
                crocoCtrl.OnEndGameEvent -= EndGame;
                hisnpaPrefab.SetActive(false);
                isEventOpen = false;
            }
        }
    }
}