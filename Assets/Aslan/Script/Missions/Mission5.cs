using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using Hsinpa.Ctrl;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Mission5 : Mission
    {
        public GameObject hisnpaPrefab;
        public RhinoBoneRepairCtrl rhinoCtrl;
        private int missionIndex = 5;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();
        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);

            hisnpaPrefab.SetActive(true);
            rhinoCtrl.OnEndGameEvent += EndGame;
            rhinoCtrl.EnterGame();
        }

        public void EndGame(bool isSuccess)
        {
            //===TypeFlag.NotifyType type = isSuccess ? TypeFlag.NotifyType.SuccessDialog : TypeFlag.NotifyType.FailDialog;
            rhinoCtrl.OnEndGameEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            var model = GameModals.instance.OpenModal<DialogModal>();
            //===model.ShowInfo(missionIndex, type);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                //Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}
