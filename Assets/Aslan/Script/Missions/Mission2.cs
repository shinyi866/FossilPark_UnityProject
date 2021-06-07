using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission2 : Mission
    {
        public FossilClass fossilClass;
        private Game2 game;
        private int missionIndex = 2;
        private bool isARsupport;

        public override void EnterGame()
        {
            isARsupport = MainApp.Instance.isARsupport;

            game = Games.instance.OpenGame<Game2>();
            game.Init();
            fossilClass.gameOverEvent += AREndGame;

            if (isARsupport)
                GameModals.instance.OpenAR();
            else
            {
                MediaPlayerController.instance.OpenSphereVideo(true);
                Modals.instance.CloseAllModal();
            }                
        }

        public override void StartGame()
        {
            game.GameStart(isARsupport);
        }

        public void AREndGame(bool isSuccess)
        {
            fossilClass.gameOverEvent -= AREndGame;
            SoundPlayerController.Instance.StopSoundEffect();

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, TypeFlag.DialogType.EndDialog);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();

                if (isARsupport)
                    GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
                else
                    GameModals.instance.GetBackAnimalNoAR(missionIndex);

            });
        }
    }
}
