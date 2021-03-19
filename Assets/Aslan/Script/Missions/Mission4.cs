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

            var model = GameModals.instance.OpenModal<DialogModal>();
            //model.ShowInfo(missionIndex, TypeFlag.NotifyType.GameNotify);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                GameModals.instance.CloseModal();
                game.GameStart();
            });
        }

        public override void StartGame()
        {

        }

        public void EndGame(bool isSuccess)
        {
            //TypeFlag.NotifyType type = true;// isSuccess ? TypeFlag.NotifyType.SuccessDialog : TypeFlag.NotifyType.FailDialog;
            game.gameOverEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            var model = GameModals.instance.OpenModal<DialogModal>();
            //model.ShowInfo(missionIndex, type);
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
