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

        public static bool backToDinosaur;

        public override void EnterGame()
        {
            
        }

        public override void StartGame()
        {
            GameModals.instance.OpenAR();
            CameraCtrl.instance.OcclusionForHuman();

            backToDinosaur = true;
            game = Games.instance.OpenGame<Game8>();
            game.Init();
            //game.gameOverEvent += EndGame;
            game.GameStart();
        }

        public void EndGame(bool isSuccess)
        {

        }
    }
}
