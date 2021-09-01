﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission4 : Mission
    {
        private Game4 game;

        private int missionIndex = 4;
        private bool isARsupport;

        public override void EnterGame()
        {
            MediaPlayerController.instance.SetUp();

            game = Games.instance.OpenGame<Game4>();
            game.Init();
            game.gameOverEvent += EndGame;
        }

        public override void StartGame()
        {
            game.GameStart();
        }

        public void EndGame(bool isSuccess)
        {
            game.gameOverEvent -= EndGame;
            GC.Collect();

            var modal = GameModals.instance.OpenModal<PictureModal>();
            modal.ShowInfo(missionIndex, TypeFlag.PictureType.EndGuide);
            modal.GuideConfirmButtonTwo.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                SoundPlayerController.Instance.PlayBackgroundMusic();
                GameModals.instance.CloseModal();

                if (MainApp.Instance.isARsupport)
                    GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                else
                    GameModals.instance.GetBackAnimalNoAR(missionIndex);
            });
        }
    }
}
