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
        [Header("GameObject")]
        [SerializeField]
        private GameObject fossilDolphin;
        [SerializeField]
        private GameObject fossilBaleenWhale;

        [Header("Transform")]
        [SerializeField]
        private Transform Container1;
        [SerializeField]
        private Transform Container2;
        [SerializeField]
        private Transform AnsContainer;
        [SerializeField]
        private Transform PlaceImage1;
        [SerializeField]
        private Transform PlaceImage2;
        [SerializeField]
        private Transform AnsImage1;
        [SerializeField]
        private Transform AnsImage2;

        [Header("Button")]
        [SerializeField]
        private GameObject button;

        [Header("Canvas")]
        [SerializeField]
        private GameObject canvas;

        [SerializeField, Range(10f, 60f)]
        private float errorRound = 40f;

        public System.Action<bool> gameOverEvent;
        public static int AnsInBoxCount;

        private bool isGameStart;
        private bool finishGame;
        private int missionIndex = 4;

        private List<GameObject> AnsBox = new List<GameObject>();
        private List<GameObject> Ans = new List<GameObject>();
        private List<Vector2> AnsOrgPos = new List<Vector2>(); // reset ans original position
        private List<string> ans1 = new List<string> { "有牙齒", "掠食性", "一個外鼻孔" };
        private List<string> ans2 = new List<string> { "沒有牙齒", "濾食性", "兩個外鼻孔" };

        private Camera _camera;

        private string videoPath = "Video/shale.mp4";
        private string successVidePath = "Video/dolphin360.mp4";

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();

            Modals.instance.CloseAllModal();
            MediaPlayerController.instance.LoadVideo(videoPath);
        }

        public void GameStart()
        {
            isGameStart = true;
            button.SetActive(false);
            canvas.SetActive(true);

            var modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Original);
            

            button.GetComponent<Button>().onClick.AddListener(()=>
            {
                var reault = CheckAnswer();
                finishGame = reault;

                if (!reault)
                {
                    //ReSetPosition();
                    SoundPlayerController.Instance.ErrorSoundEffect();
                }
                else
                {
                    Object.SetActive(false);
                    GameModals.instance.CloseModal();
                    //MediaPlayerController.instance.CloseVideo();
                    MediaPlayerController.instance.LoadAndPlayVideoNotLoop(successVidePath);
                    SoundPlayerController.Instance.PauseBackgroundMusic();
                    SoundPlayerController.Instance.FInishAllSoundEffect();
                    finishGame = true;
                }
            });

            SetPlacePosition();
            SetAnsPosition();
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private void SetPlacePosition()
        {
            float height = 80f;
            float containHeight = 160f;

            for (int i = 0; i < 3; i++)
            {
                Transform rankTransform = Instantiate(PlaceImage1, Container1);
                RectTransform rankRectTransform = rankTransform.GetComponent<RectTransform>();
                RectTransform containRect = Container1.GetComponent<RectTransform>();

                Transform rankTransform2 = Instantiate(PlaceImage2, Container2);
                RectTransform rankRectTransform2 = rankTransform2.GetComponent<RectTransform>();
                RectTransform containRect2 = Container2.GetComponent<RectTransform>();

                containRect.sizeDelta = new Vector2(0, 400);
                containRect2.sizeDelta = new Vector2(0, 400);
                rankRectTransform.anchoredPosition = new Vector2(6, containHeight - height * (i + 1));
                rankRectTransform2.anchoredPosition = new Vector2(6, containHeight - height * (i + 1));

                AnsBox.Add(rankTransform.gameObject);
                AnsBox.Add(rankTransform2.gameObject);
                rankTransform.gameObject.SetActive(true);
                rankTransform2.gameObject.SetActive(true);
            }
        }

        private void SetAnsPosition()
        {
            float contain = 60f;
            float vertical = 60f;
            float revertical = 130f;

            for (int i = 0; i < 3; i++)
            {
                Transform ansTransform = Instantiate(AnsImage1, AnsContainer);
                RectTransform ansRectTransform = ansTransform.GetComponent<RectTransform>();
                RectTransform containRect = AnsContainer.GetComponent<RectTransform>();

                containRect.sizeDelta = new Vector2(400, 300);

                var setPos = new Vector2(700f, contain - revertical * (i + 1) + 500f);
                ansRectTransform.anchoredPosition = new Vector2(-95f, contain - vertical * (i + 1));
                ansRectTransform.GetComponentInChildren<Text>().text = ans1[i];

                AnsOrgPos.Add(setPos);
                Ans.Add(ansTransform.gameObject);
                ansTransform.gameObject.SetActive(true);
            }

            for (int i = 0; i < 3; i++)
            {
                Transform ansTransform2 = Instantiate(AnsImage2, AnsContainer);
                RectTransform ansRectTransform2 = ansTransform2.GetComponent<RectTransform>();
                RectTransform containRect2 = AnsContainer.GetComponent<RectTransform>();

                containRect2.sizeDelta = new Vector2(400, 300);

                var setPos = new Vector2(1110f, contain - revertical * (i + 1) + 500f); 
                ansRectTransform2.anchoredPosition = new Vector2(125f, contain - vertical * (i + 1));
                ansRectTransform2.GetComponentInChildren<Text>().text = ans2[i];

                AnsOrgPos.Add(setPos);
                Ans.Add(ansTransform2.gameObject);
                ansTransform2.gameObject.SetActive(true);
            }

            RandomAnsPosition();
        }

        private void RandomAnsPosition()
        {
            for (int i = 0; i < 6; i++)
            {
                var r1 = Random.Range(0, Ans.Count);
                var r2 = Random.Range(0, Ans.Count);

                var pos1 = Ans[r1].transform.position;
                Ans[r1].transform.position = Ans[r2].transform.position;
                Ans[r2].transform.position = pos1;
            }
        }

        private void ReSetPosition()
        {
            for(int i = 0; i < AnsOrgPos.Count; i++)
            {
                for (int j = 0; j < Ans.Count; j++)
                {
                    if(i == j)
                    {
                        Ans[j].transform.position = AnsOrgPos[i];
                    }
                }
            }

            RandomAnsPosition();
        }


        private void DetectAnswer()
        {
            for (int i = 0; i < AnsBox.Count; i++)
            {
                for (int j = 0; j < Ans.Count; j++)
                {                     
                    if (Vector2.Distance(AnsBox[i].transform.position, Ans[j].transform.position) < errorRound)
                    {
                        Ans[j].transform.position = AnsBox[i].transform.position;
                    }
                }
            }
        }

        private bool CheckAnswer()
        {
            bool result = false;
            int countRight = 0 ;

            if (AnsInBoxCount != 6) { result = false; }

            for (int i = 0; i < AnsBox.Count; i++)
            {
                for (int j = 0; j < Ans.Count; j++)
                {
                    if (Vector3.Distance(AnsBox[i].transform.position, Ans[j].transform.position) < errorRound)
                    {
                        Ans[j].GetComponent<Image>().color = Color.white;

                        if (i % 2 == 0 && j < Ans.Count / 2)
                        {
                            Debug.Log("Bingo!");
                            countRight++;
                        }
                        else if (i % 2 != 0 && j >= Ans.Count / 2)
                        {
                            Debug.Log("Bingo!!!!!");
                            countRight++;
                        }
                        else
                        {
                            Ans[j].GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
            
            if (countRight == 6) { result = true; }

            return result;
        }

        private void SetBonePosition()
        {
            var _cameraFront = _camera.transform.forward;
            var _frontPos = _cameraFront * 3;
            var _leftPos = _camera.transform.right * -1.6f;
            var _rightPos = _camera.transform.right * 1.6f;
            var _upPos = _camera.transform.up * 0.7f;

            var rotateSpeed = 20f * Time.deltaTime;

            fossilDolphin.transform.position = _camera.transform.position + _frontPos + _leftPos + _upPos;
            fossilDolphin.transform.Rotate(0, rotateSpeed, 0);

            fossilBaleenWhale.transform.position = _camera.transform.position + _frontPos + _rightPos + _upPos;
            fossilBaleenWhale.transform.Rotate(0, rotateSpeed, 0);
        }

        private void Update()
        {
            if (!isGameStart) return;

            if (!finishGame)
            {
                DetectAnswer();
                SetBonePosition();
            }
            
            if (AnsInBoxCount == 6) { button.SetActive(true); }
            else { button.SetActive(false); Debug.Log("Ans Count " + AnsInBoxCount); }

            if (MediaPlayerController.instance.isVideoFinish() && finishGame) // TODO: event?
            {
                GameResult(true);
                isGameStart = false;
            }
        }
    }
}
