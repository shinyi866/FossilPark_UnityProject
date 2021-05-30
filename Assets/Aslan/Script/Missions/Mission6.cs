using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission6 : Mission
    {
        //public Material material;

        private Game6 game;
        private int missionIndex = 6;
        private Material currentMaterial;

        public override void EnterGame()
        {
            game = Games.instance.OpenGame<Game6>();
            game.Init();
            game.gameOverEvent += EndGame;
        }

        public override void StartGame()
        {
            SoundPlayerController.Instance.PauseBackgroundMusic();
            game.GameStart();

            currentMaterial = RenderSettings.skybox;
            //RenderSettings.skybox = material;
        }

        public void EndGame(bool isSuccess)
        {
            game.gameOverEvent -= EndGame;
            RenderSettings.skybox = currentMaterial;

            var modal = GameModals.instance.OpenModal<PictureModal>();
            modal.ShowInfo(missionIndex, TypeFlag.PictureType.EndGuide);
            modal.GuideConfirmButtonTwo.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
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