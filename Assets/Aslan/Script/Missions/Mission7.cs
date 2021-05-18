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
        public ARPlaneManager planeManager;

        private int missionIndex = 7;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();
            if (!MainApp.Instance.isARsupport) { CameraCtrl.instance.SwitchCameraForHsinpaMission(true); } //Ar support for hsinpa check

            planeManager.enabled = true;
            hisnpaPrefab.SetActive(true);
        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);
            
            crocoCtrl.OnEndGameEvent += EndGame;
            crocoCtrl.EnterGame(15, MainApp.Instance.isARsupport);
        }

        public void EndGame(bool isSuccess)
        {
            crocoCtrl.OnEndGameEvent -= EndGame;

            var ARmodal = GameModals.instance.OpenModal<ARGameModal>();
            ARmodal.ShowPanel(ARmodal.gamePromptPanel.canvasGroup, false);

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);

            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                //MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}