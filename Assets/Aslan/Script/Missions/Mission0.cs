using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission0 : Mission
    {
        private Game0 game;

        public override void EnterGame()
        {

        }

        public override void StartGame()
        {            
            GameModals.instance.OpenAR();
            Debug.Log("0");
            game = Games.instance.OpenGame<Game0>();
            game.gameOverEvent += EndGame;
            game.Init();
            game.GameStart();
        }

        public void EndGame()
        {
            game.gameOverEvent -= EndGame;
            //Games.instance.ClosGame();
            //GameModals.instance.CloseAR();
            GameModals.instance.CloseModal();
            var modal = Modals.instance.OpenModal<ARModal>();
            modal.ShowView(false);
        }
    }
}
