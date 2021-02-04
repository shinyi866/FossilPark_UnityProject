﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using Hsinpa.Ctrl;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Mission5 : Mission
    {
        public ARPlaneManager arPlaneManager;
        public GameObject plane;

        public RhinoBoneRepairCtrl rhino;
        public GameObject game;
        private int missionIndex = 5;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();
            arPlaneManager.planePrefab = plane;
            //game = Games.instance.OpenGame<Game5>();
            //game.Init();
            //game.gameOverEvent += EndGame;

            game.SetActive(true);
            rhino.OnEndGameEvent += EndGame;

            var model = GameModals.instance.OpenModal<NotifyModal>();
            model.ShowInfo(missionIndex, TypeFlag.NotifyType.GameNotify);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                var gameModal = GameModals.instance.OpenModal<ARGameModal>();
                gameModal.ShowModal(TypeFlag.ARGameType.Game5);
                rhino.EnterGame();
            });
        }

        public void EndGame(bool isSuccess)
        {
            arPlaneManager.planePrefab = null;

            TypeFlag.DialogType type = isSuccess ? TypeFlag.DialogType.SuccessDialog : TypeFlag.DialogType.FailDialog;
            rhino.OnEndGameEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, type);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}
