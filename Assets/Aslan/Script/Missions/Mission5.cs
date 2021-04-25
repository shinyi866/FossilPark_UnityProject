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
        public ARPlaneManager planeManager;

        private int missionIndex = 5;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();
            planeManager.enabled = true;
            hisnpaPrefab.SetActive(true);
        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);
            
            rhinoCtrl.OnEndGameEvent += EndGame;
            rhinoCtrl.EnterGame(100);
        }

        public void EndGame(bool isSuccess)
        {
            rhinoCtrl.OnEndGameEvent -= EndGame;
            var data = MainApp.Instance.database;
            planeManager.enabled = false;

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, TypeFlag.DialogType.EnterDialog);
            model.message.text = data.m_Data[missionIndex].endMessage[0];
            model.NextButton.onClick.AddListener(() =>
            {
                model.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);
                model.logoImage.enabled = true;
                model.message.text = data.m_Data[missionIndex].endMessage[1];

                model.ConfirmButton.onClick.AddListener(() =>
                {
                    model.logoImage.enabled = false;
                    //Games.instance.ClosGame();
                    MediaPlayerController.instance.CloseVideo();
                    GameModals.instance.CloseModal();
                    GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                });
            });
        }
    }
}
