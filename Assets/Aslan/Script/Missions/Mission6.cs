﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission6 : Mission
    {
        public Material material;

        private Game6 game;
        private int missionIndex = 6;
        private Material currentMaterial;

        public override void EnterGame()
        {
            MediaPlayerController.instance.SetUp();

            game = Games.instance.OpenGame<Game6>();
            game.Init();
            game.gameOverEvent += EndGame;
        }

        public override void StartGame()
        {
            SoundPlayerController.Instance.PauseBackgroundMusic();
            game.GameStart();

            currentMaterial = RenderSettings.skybox;
            RenderSettings.skybox = material;
        }

        public void EndGame(bool isSuccess)
        {
            game.gameOverEvent -= EndGame;
            RenderSettings.skybox = currentMaterial;

            var modal = GameModals.instance.OpenModal<PictureModal>();
            modal.ShowInfo(missionIndex, TypeFlag.PictureType.EndGuide);
            modal.GuideConfirmButtonTwo.onClick.AddListener(() =>
            {
                if (MainApp.Instance.isARsupport)
                    MediaPlayerController.instance.DestroyVideo();

                Games.instance.ClosGame();                
                SoundPlayerController.Instance.PlayBackgroundMusic();
                GameModals.instance.CloseModal();
                
                if (MainApp.Instance.isARsupport)
                    GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                else
                    GameModals.instance.GetBackAnimalNoAR(missionIndex);
            });
        }

        public void BackToMain()
        {
            if(currentMaterial != null)
                RenderSettings.skybox = currentMaterial;

            SoundPlayerController.Instance.PlayBackgroundMusic();
        }
    }
}