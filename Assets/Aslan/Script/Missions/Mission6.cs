using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission6 : Mission
    {
        private Game6 game;
        private int missionIndex = 6;

        public override void EnterGame()
        {
            game = Games.instance.OpenGame<Game6>();
            game.Init();
            game.gameOverEvent += EndGame;

            var model = GameModals.instance.OpenModal<TitleModal>();
            model.ShowInfo(missionIndex, TypeFlag.TitleType.GameTitle);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                var gameModel = GameModals.instance.OpenModal<PictureModal>();
                gameModel.ShowInfo(missionIndex, TypeFlag.PictureType.MissionType);
                game.GameStart();
            });
        }

        public override void StartGame()
        {

        }

        public void EndGame(bool isSuccess)
        {
            //===TypeFlag.NotifyType type = isSuccess ? TypeFlag.NotifyType.SuccessDialog : TypeFlag.NotifyType.FailDialog;
            game.gameOverEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            var model = GameModals.instance.OpenModal<DialogModal>();
            //===model.ShowInfo(missionIndex, type);
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