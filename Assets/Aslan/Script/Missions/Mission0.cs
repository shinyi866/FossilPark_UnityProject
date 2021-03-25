using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission0 : Mission
    {
        private Game0 game;
        private int missionIndex = 0;

        public override void EnterGame()
        {

        }

        public override void StartGame()
        {
            GameModals.instance.OpenAR();

            game = Games.instance.OpenGame<Game0>();
            game.Init();
            game.GameStart();
            Debug.Log("0");
        }
    }
}
