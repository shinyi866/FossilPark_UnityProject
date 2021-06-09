using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission1 : Mission
    {
        private Game1 game;
        private int missionIndex = 1;

        public override void EnterGame()
        {
            
        }

        public override void StartGame()
        {
            MediaPlayerController.instance.SetUp();

            game = Games.instance.OpenGame<Game1>();
            //game.gameOverEvent += EndGame;
            game.Init();
            game.GameStart();
        }

        public void EndGame()
        {
        }
    }
}

