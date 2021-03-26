using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission7 : Mission
    {

        private int missionIndex = 7;

        public override void EnterGame()
        {

        }

        public override void StartGame()
        {
            var modal = GameModals.instance.OpenModal<ARGameModal>();

            // step 1
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt1);

            // step 2
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt2);

            // step 3
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt3);

            // step 4
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt4);

            // step 5
            modal.ShowPrompt(missionIndex, TypeFlag.ARGameType.GamePrompt5);
        }

        public void EndGame(bool isSuccess)
        {

        }
    }
}
