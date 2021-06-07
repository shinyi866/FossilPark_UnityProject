using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace GameMission
{
    public class Game0 : Game
    {
        [SerializeField]
        private GameObject ClockObject;
        [SerializeField]
        private GameObject DirectObject;
        [SerializeField]
        private Text text;
        [SerializeField]
        private GameObject image;

        //public System.Action gameOverEvent;

        private float time = 2;
        private int missionIndex;
        private Camera _camera;
        private bool isGameStart;
        private bool placeClock;
        private ARGameModal modal;
        private string videoPath = "Video/clock_360.mp4";

        private string crocString = "臺灣古鱷\n臺南左鎮菜寮溪三重溪的河床露頭所發現的「臺灣古鱷」，具有較長的嘴吻，擁有圓錐型的同型齒。化石陳列於園區第四館。";
        private string deerString = "台灣梅花鹿\n梅花鹿一般棲息於針括混合林，山地草原，和森林邊緣附近。化石陳列於園區第四館。";
        private string dolphinString = "海豚\n廣泛分布於西太平洋和東印度洋的熱帶及溫帶沿岸海域，偶爾出現在淡水中。化石陳列於園區第四館。";
        private string elephantString = "草原猛獁象\n起源於中國大陸泥河灣地區，較屬於草原型的環境。化石陳列於園區第四館。";
        private string monkeyString = "金絲猴\n由於世界上最早發現的仰鼻猴是長有金黃色皮毛、生活在中國的四川、陝西、甘肅的川金絲猴。化石陳列於園區第四館。";
        private string rhinoString = "早坂犀牛\n比過去從中國大陸所產的中國犀牛及其他幾種犀牛時代更早，體形更小，可謂中國犀牛的祖先型。化石陳列於園區第四館。";

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();

            if(!MainApp.Instance.isARsupport)
                MediaPlayerController.instance.LoadVideo(videoPath);

            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game0);
            ClockObject.SetActive(false);

            modal.game0Panel.button_go.onClick.AddListener(()=> {
                RaycastHit hit;
                placeClock = true;
                ClockObject.SetActive(true);
                modal.CloseAllPanel();
                GameModals.instance.CloseModal();
                Modals.instance.OpenModal<ARModal>();
                var mainModals = Modals.instance.GetModel<MainModal>();
                mainModals.ShowPanel(mainModals.promptView, true);
                iBeaconMissionSetting.Instance.isEnterGame = true;
                //ARmodal.ShowView(false);
                //GameResult();
                /*
                if (Physics.Raycast(transform.position, _camera.transform.forward, out hit, 3))
                {
                    placeClock = true;
                    ClockObject.SetActive(true);
                    modal.CloseAllPanel();
                    GameResult();
                }
                else
                {
                    modal.ShowPrompt(0, TypeFlag.ARGameType.GamePrompt1);
                    modal.gamePromptPanel.image.sprite = MainApp.Instance.database.m_Data[0].animalDialogPicture;
                }*/
            });

            
        }

        public void GameStart()
        {
            isGameStart = true;
        }

        private void ResetDirection()
        {
            if (time > 0)
            {
                Compass.Instance.SetUp(DirectObject, 270);
                DirectObject.transform.position = new Vector3(0, 0, 3);
                time -= Time.deltaTime;
            }
        }

        void Update()
        {
            if (!isGameStart) return;

            ResetDirection();

            if (!placeClock)
            {
                var _cameraFront = _camera.transform.forward;
                var _cameraUp = _camera.transform.up;
                var _frontPos = _cameraFront * 3;
                var _downPos = _cameraUp * -0.5f;

                //_cameraFront.y = 0;
                ClockObject.transform.position = _camera.transform.position + _frontPos + _downPos;
                ClockObject.transform.rotation = Quaternion.LookRotation(_cameraFront);
            }
        }

        // Show clock objecy info
        public void ShowInfo(GameObject gameObject)
        {
            var name = gameObject.name;
            image.SetActive(true);

            if (name == "croc2")
            {
                text.text = crocString;
            }

            if (name == "DEER2")
            {
                text.text = deerString;
            }

            if (name == "dolphin2")
            {
                text.text = dolphinString;
            }

            if (name == "elephant2")
            {
                text.text = elephantString;
            }

            if (name == "monkey4")
            {
                text.text = monkeyString;
            }

            if (name == "rhino2")
            {
                text.text = rhinoString;
            }
        }
    }
}

