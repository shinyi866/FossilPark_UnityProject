using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission8 : Mission
    {
        private Game8 game;
        private int missionIndex = 8;

        public override void EnterGame()
        {
            GameModals.instance.OpenAR();

            game = Games.instance.OpenGame<Game8>();
            game.Init();
            game.gameOverEvent += EndGame;

            var model = GameModals.instance.OpenModal<NotifyModal>();
            model.ShowInfo(missionIndex, TypeFlag.NotifyType.GameNotify);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                var gameModal = GameModals.instance.OpenModal<ARGameModal>();
                gameModal.ShowModal(TypeFlag.ARGameType.Game7);
                game.GameStart();
        });
        }

        public void EndGame(bool isSuccess)
        {

        }
    }
}
