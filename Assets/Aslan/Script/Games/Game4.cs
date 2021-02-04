using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using View;

namespace GameMission
{
    public class Game4 : Game
    {
        public System.Action<bool> gameOverEvent;

        [Header("限時幾秒")]
        public float OverTime = 10;

        float VideoTime = 12;
        [Header("計時UI")]
        public Text timeUI;
        float f;
        [Header("進度條")]
        public RectTransform strip;
        public GameObject gameUI;

        private bool isGameStart;
        private int passClick = 30;

        private string videoPath = "Video/LadyVisit.mp4";
        private string successVidePath = "Video/4096.mp4";
        private string failVidePath = "AVProVideoSamples/SampleSphere.mp4";

        public void Init()
        {
            Modals.instance.CloseAllModal();
            MediaPlayerController.instance.LoadVideo(videoPath);
        }

        public void GameStart()
        {
            isGameStart = true;
            MediaPlayerController.instance.PlayVideo();
        }

        public void UI_Enter()
        {
            if (f <= passClick) { f++; }
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        void Update()
        {
            if (!isGameStart) return;

            if (f >= passClick)
            {
                VideoTime = 0;
                MediaPlayerController.instance.LoadAndPlayVideo(successVidePath);

                gameUI.SetActive(false);
                isGameStart = false;
                GameResult(true);
            }
            else
            {
                if (VideoTime > 0)
                {
                    VideoTime -= Time.deltaTime;
                    timeUI.text = Mathf.RoundToInt(VideoTime).ToString();
                    strip.localScale = new Vector3(0 + (f / passClick), 1, 1);
                }
                else
                {
                    bool isSuccess = f >= passClick;

                    if (f >= passClick)
                    {
                        MediaPlayerController.instance.LoadAndPlayVideo(successVidePath);
                    }
                    else
                    {
                        MediaPlayerController.instance.LoadAndPlayVideo(failVidePath);
                    }

                    gameUI.SetActive(false);
                    isGameStart = false;
                    GameResult(isSuccess);
                }
            }

        }
    }
}
