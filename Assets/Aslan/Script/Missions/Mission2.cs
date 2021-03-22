using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace GameMission
{
    public class Mission2 : Mission
    {
        private Game2 game;
        private int missionIndex = 2;
        private bool isARsupport;
        private string videoPath = "Video/4096.mp4";

        public override void EnterGame()
        {
            isARsupport = MainApp.Instance.isARsupport;

            game = Games.instance.OpenGame<Game2>();
            game.Init();
            game.gameOverEvent += EndGame;

            if(!isARsupport)
            {
                MediaPlayerController.instance.LoadVideo(videoPath);
                MediaPlayerController.instance.PlayVideo();
            }
            else
            {
                GameModals.instance.OpenAR();
            }

            //game.GameStart(MainApp.Instance.isARsupport);
            /*
            var model = GameModals.instance.OpenModal<TitleModal>();
            model.ShowInfo(missionIndex, TypeFlag.TitleType.GameTitle);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                Debug.Log("enter");
                //var gameModel = GameModals.instance.OpenModal<PictureModal>();
                //gameModel.ShowInfo(missionIndex, TypeFlag.PictureType.MissionType);
                game.GameStart();
            });
            */
        }

        public override void StartGame()
        {
            game.GameStart(isARsupport);
        }

        public void EndGame(bool isSuccess)
        {
            TypeFlag.DialogType type = isSuccess ? TypeFlag.DialogType.EndDialog : TypeFlag.DialogType.FailDialog;
            game.gameOverEvent -= EndGame;

            if (isSuccess)
                MainApp.Instance.Score();

            

            var model = GameModals.instance.OpenModal<DialogModal>();
            model.ShowInfo(missionIndex, type);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                MediaPlayerController.instance.CloseVideo();
                GameModals.instance.CloseModal();
                GameModals.instance.GetBackAnimalAR(missionIndex, TypeFlag.ARObjectType.Animals);
            });
        }
    }
}
