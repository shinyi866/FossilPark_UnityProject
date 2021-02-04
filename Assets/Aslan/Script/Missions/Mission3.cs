using System.Collections;
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
            GameModals.instance.OpenAR();

            game = Games.instance.OpenGame<Game3>();
            game.Init();
            game.gameOverEvent += EndGame;

            var model = GameModals.instance.OpenModal<NotifyModal>();
            model.ShowInfo(missionIndex, TypeFlag.NotifyType.GameNotify);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                var gameModal = GameModals.instance.OpenModal<ARGameModal>();
                gameModal.ShowModal(TypeFlag.ARGameType.Game3);
                game.GameStart();
            });
        }

        public void EndGame(bool isSuccess)
        {
            TypeFlag.DialogType type = isSuccess ? TypeFlag.DialogType.SuccessDialog : TypeFlag.DialogType.FailDialog;
            game.gameOverEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, type);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}
