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
        private GameObject answerModel;

        [SerializeField]
        private GameObject answerBoxModel;

        [SerializeField]
        private GameObject dolphinContainer;

        [SerializeField]
        private GameObject whaleContainer;

        [SerializeField]
        private GameObject answerContainer;

        [SerializeField]
        private Material[] material; // 0: true, 1: false. 2: normal

        [SerializeField]
        private Material[] materialAns; // 0: blue, 1:yellow

        [SerializeField, Range(0f, 1f)]
        private float errorRound = 0.2f;

        public System.Action<bool> gameOverEvent;

        private bool isGameStart;
        private bool finishGame;
        private float downTime = 3;
        private int boxAnswerNumber = 3;

        private List<GameObject> AnsBox = new List<GameObject>();
        private List<GameObject> Ans = new List<GameObject>();

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
            Vector3 transform;
            var _cameraFront = _camera.transform.forward;
            
            var fossilDolphinPos = _cameraFront + new Vector3(-1.5f, 0.8f, 3f);
            fossilDolphin.transform.position = _camera.transform.position + fossilDolphinPos;
            dolphinContainer.transform.position = new Vector3(fossilDolphin.transform.position.x, fossilDolphin.transform.position.y, fossilDolphin.transform.position.z);

            for (int i = 1; i <= boxAnswerNumber; i++)
            {
                GameObject clone = Instantiate(answerBoxModel, new Vector3(dolphinContainer.transform.position.x, dolphinContainer.transform.position.y - 0.5f * i, dolphinContainer.transform.position.z) , dolphinContainer.transform.rotation);
                clone.transform.SetParent(dolphinContainer.transform);
                clone.SetActive(true);

                //fossilDolphinBox.Add(clone);
                AnsBox.Add(clone);
            }

            var fossilBaleenWhalePos = _cameraFront + new Vector3(1.5f, 0.8f, 3f);
            fossilBaleenWhale.transform.position = _camera.transform.position + fossilBaleenWhalePos;
            whaleContainer.transform.position = new Vector3(fossilBaleenWhale.transform.position.x, fossilBaleenWhale.transform.position.y, fossilBaleenWhale.transform.position.z);

            for (int i = 1; i <= boxAnswerNumber; i++)
            {
                GameObject clone = Instantiate(answerBoxModel, new Vector3(whaleContainer.transform.position.x, whaleContainer.transform.position.y - 0.5f * i, whaleContainer.transform.position.z), whaleContainer.transform.rotation);
                clone.transform.SetParent(whaleContainer.transform);
                clone.SetActive(true);

                //fossilBaleenWhaleBox.Add(clone);
                AnsBox.Add(clone);
            }
            
            var answerPos = _cameraFront + new Vector3(0f, -1.8f, 1f);

            for (int i = 1; i <= boxAnswerNumber*2; i++)
            {
                GameObject clone = Instantiate(answerModel, answerContainer.transform);
                if (i < 4)
                {
                    clone.GetComponent<MeshRenderer>().material = materialAns[0];
                    //fossilDolphinAns.Add(clone);
                    Ans.Add(clone);
                }
                else
                {
                    clone.GetComponent<MeshRenderer>().material = materialAns[1];
                    //fossilBaleenWhaleAns.Add(clone);
                    Ans.Add(clone);
                }
                clone.transform.position = RandomPosition(answerPos, new Vector3(3f, 1f, 1f));
                clone.SetActive(true);
            }
        }

        private Vector3 RandomPosition(Vector3 center, Vector3 size)
        {
            return center + new Vector3((Random.value - 0.5f) * size.x,(Random.value - 0.5f) * size.y, center.z);
        }
        
        private void DetectAnswer()
        {
            float distance = 0;

            for (int i = 0; i < AnsBox.Count; i++)
            {
                for (int j = 0; j < Ans.Count; j++)
                {
                    if (Vector3.Distance(AnsBox[i].transform.position, Ans[j].transform.position) < errorRound)
                    {
                        //Debug.Log("in");
                        
                        Ans[j].transform.position = AnsBox[i].transform.position;

                        if(i == j)
                        {
                            Ans[j].GetComponent<MeshRenderer>().material = material[0];
                            Ans[j].tag = "Untagged";
                            distance = 0;
                            Debug.Log("Bingo!");
                        }
                        if (i != j)
                        {
                            Ans[j].GetComponent<MeshRenderer>().material = material[1];
                            distance += Vector3.Distance(AnsBox[i].transform.position, Ans[j].transform.position);
                        }
                    }
                    else
                    {
                        distance += Vector3.Distance(AnsBox[i].transform.position, Ans[j].transform.position);

                        if (i == 0)
                        {
                            if (j < 3)
                                Ans[j].GetComponent<MeshRenderer>().material = materialAns[0];
                            else
                                Ans[j].GetComponent<MeshRenderer>().material = materialAns[1];
                        }
                    }
                }
            }
            
            if (distance == 0) // all error can in or very close
            {
                downTime -= Time.deltaTime;
                Debug.Log("finish!!!");
                if (downTime < 0)
                {
                    SetPosition();
                    Object.SetActive(false);

                    MediaPlayerController.instance.CloseVideo();
                    MediaPlayerController.instance.LoadAndPlayVideoNotLoop(successVidePath);
                    finishGame = true;
                }
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
