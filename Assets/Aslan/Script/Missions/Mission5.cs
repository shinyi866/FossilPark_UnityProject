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
#if UNITY_IOS
                CameraCtrl.instance.OpenARPlaneManager(true);
#endif
            }

            hisnpaPrefab.SetActive(true);
        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);

            isEventOpen = true;
            rhinoCtrl.OnEndGameEvent += EndGame;
            rhinoCtrl.EnterGame(87, MainApp.Instance.isARsupport);
        }

        public void EndGame(bool isSuccess)
        {
            isEventOpen = false;
            rhinoCtrl.OnEndGameEvent -= EndGame;
            var data = MainApp.Instance.database;
            CameraCtrl.instance.OpenARPlaneManager(false);

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
                    //MediaPlayerController.instance.CloseVideo();
                    GameModals.instance.CloseModal();

                    if (MainApp.Instance.isARsupport)
                        GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                    else
                        GameModals.instance.GetBackAnimalNoAR(missionIndex);
                });
            });
        }

        public void BackToMain()
        {
            if (isEventOpen)
            {
                rhinoCtrl.OnEndGameEvent -= EndGame;
                rhinoCtrl.CleanBone();
                hisnpaPrefab.SetActive(false);
                isEventOpen = false;
            }
            
        }
    }
}
