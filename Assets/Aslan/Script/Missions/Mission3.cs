﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission3 : Mission
    {
        private Game3 game;
        private int missionIndex = 3;

        public override void EnterGame()
        {
            if (!MainApp.Instance.isARsupport)
                MediaPlayerController.instance.SetUp();

            GameModals.instance.OpenAR();

            game = Games.instance.OpenGame<Game3>();
            game.Init();
            game.gameOverEvent += EndGame;
        }

        public override void StartGame()
        {
            game.GameStart();
        }

        public void EndGame(bool isSuccess)
        {
            TypeFlag.DialogType type = isSuccess ? TypeFlag.DialogType.EndDialog : TypeFlag.DialogType.FailDialog;
            game.gameOverEvent -= EndGame;

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, type);
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
    }
}
