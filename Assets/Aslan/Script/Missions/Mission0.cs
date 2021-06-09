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
            if(!MainApp.Instance.isARsupport)
                MediaPlayerController.instance.SetUp();

            GameModals.instance.OpenAR();
            
            game = Games.instance.OpenGame<Game0>();
            //game.gameOverEvent += EndGame;
            game.Init();
            game.GameStart();
        }

        public void EndGame()
        {
            //game.gameOverEvent -= EndGame;
            //GameModals.instance.CloseModal();
            //var modal = Modals.instance.OpenModal<ARModal>();
            //modal.ShowView(false);
        }
    }
}
