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
            /*
            GameModals.instance.OpenAR();
            
            var model = GameModals.instance.OpenModal<TitleModal>();
            model.ShowInfo(missionIndex, TypeFlag.TitleType.GameTitle);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                var gameModal = GameModals.instance.OpenModal<ARGameModal>();
                gameModal.ShowModal(missionIndex, TypeFlag.ARGameType.Game8);
                game.GameStart();
            });
            */
        }

        public override void StartGame()
        {
            GameModals.instance.OpenAR();
            Debug.Log("8");
            game = Games.instance.OpenGame<Game8>();
            game.Init();
            game.gameOverEvent += EndGame;
            game.GameStart();
        }

        public void EndGame(bool isSuccess)
        {

        }
    }
}
