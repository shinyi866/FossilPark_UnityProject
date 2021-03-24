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

        public override void EnterGame()
        {
            game = Games.instance.OpenGame<Game4>();
            game.Init();
            game.gameOverEvent += EndGame;

            /*
            var model = GameModals.instance.OpenModal<DialogModal>();
            //model.ShowInfo(missionIndex, TypeFlag.NotifyType.GameNotify);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                GameModals.instance.CloseModal();
                game.GameStart();
            });
            */
        }

        public override void StartGame()
        {
            game.GameStart();
        }

        public void EndGame(bool isSuccess)
        {
            game.gameOverEvent -= EndGame;

            var modal = GameModals.instance.OpenModal<PictureModal>();
            modal.ShowInfo(missionIndex, TypeFlag.PictureType.EndGuide);
            modal.GuideConfirmButtonTwo.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}
