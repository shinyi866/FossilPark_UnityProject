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
        [SerializeField]
        private GameObject fossilDolphin;

        [SerializeField]
        private GameObject fossilBaleenWhale;

        [SerializeField]
        private GameObject answer;

        [SerializeField]
        private GameObject[] answerBox; // 0: A1, 1: A2

        [SerializeField]
        private Material[] material; // 0: true, 1: false. 2: normal

        [SerializeField, Range(0f, 1f)]
        private float errorRound = 0.1f;

        public System.Action<bool> gameOverEvent;

        private bool isGameStart;
        private bool finishGame;
        private Camera _camera;

        private string videoPath = "Video/LadyVisit.mp4";
        private string successVidePath = "Video/ele.mp4";

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();

            Modals.instance.CloseAllModal();
            MediaPlayerController.instance.LoadVideo(videoPath);
        }

        public void GameStart()
        {
            isGameStart = true;
            MediaPlayerController.instance.PlayVideo();

            SetPosition(); 
        }

        private void SetPosition()
        {

            var _cameraFront = _camera.transform.forward;
            _cameraFront.z = 3f;

            _cameraFront.y = -1.4f;
            answer.transform.position = _camera.transform.position + _cameraFront;

            _cameraFront.y = 0f;
            _cameraFront.x = -1.5f;
            fossilDolphin.transform.position = _camera.transform.position + _cameraFront;
            answerBox[0].transform.position = new Vector3(fossilDolphin.transform.position.x, -0.8f, fossilDolphin.transform.position.z);
            _cameraFront.x = 1.5f;
            fossilBaleenWhale.transform.position = _camera.transform.position + _cameraFront;
            answerBox[1].transform.position = new Vector3(fossilBaleenWhale.transform.position.x, -0.8f, fossilBaleenWhale.transform.position.z);
        }

        private void DetectAnswer()
        {
            Transform[] answersTransform = answer.GetComponentsInChildren<Transform>();
            
            for (int i = 1; i < answersTransform.Length; i++)
            {
                for (int j = 0; j < answerBox.Length; j++)
                {

                    if (Vector3.Distance(answersTransform[i].transform.position, answerBox[j].transform.position) < errorRound)
                    {
                        answersTransform[i].transform.position = answerBox[j].transform.position; 
                        Debug.Log("Fit!");

                        if (Vector3.Distance(answerBox[j].transform.position, answersTransform[j + 1].transform.position) == 0)
                        {
                            answerBox[j].GetComponent<MeshRenderer>().material = material[0];
                            Debug.Log("Bingo!");
                        }
                        else
                        {
                            answerBox[j].GetComponent<MeshRenderer>().material = material[1];
                        }
                    }
                    /*
                    else
                    {
                        answerBox[j].GetComponent<MeshRenderer>().material = material[2];
                    }
                    */
                }
            }

            if (Vector3.Distance(answerBox[0].transform.position, answersTransform[1].transform.position) == 0 && Vector3.Distance(answerBox[1].transform.position, answersTransform[2].transform.position) == 0)
            {
                SetPosition();
                Object.SetActive(false);
                MediaPlayerController.instance.CloseVideo();
                MediaPlayerController.instance.LoadAndPlayVideoNotLoop(successVidePath);
                finishGame = true;
            }
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        void Update()
        {
            if (!isGameStart) return;

            if (!finishGame) { DetectAnswer(); }

            if(MediaPlayerController.instance.isVideoFinish() && finishGame) // TODO: event?
            {
                GameResult(true);
                isGameStart = false;
            }
        }
    }
}
