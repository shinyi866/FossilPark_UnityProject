using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMission;

namespace View
{
    public class MainModal : Modal
    {
        [SerializeField]
        private CanvasGroup introView;
        
        public CanvasGroup promptView;
        [SerializeField]
        private CanvasGroup finishView;
        [SerializeField]
        private Text finishText;

        [SerializeField]
        private Image introImage;
        [SerializeField]
        private GuideView guideView;
        [SerializeField]
        private GameObject remindObject;

        [Header("Buttons")]
        [SerializeField]
        private Button[] animalButtons;
        [SerializeField]
        private Button[] remiindButtons;
        [SerializeField]
        private Button dinosaurButton;
        [SerializeField]
        private Button eggButton;
        [SerializeField]
        private Button promptButton;
        [SerializeField]
        private Button promptCloseButton;
        [SerializeField]
        private Button finishCloseButton;

        [SerializeField]
        private Button[] missionButtons;

        //[Header("Image")]
        //public Image scoreImage;
        [SerializeField]
        private Image clockImage;
        [SerializeField]
        private Sprite[] clockSprite;

        private int clockOutSide = 0;
        private int buttonIndex;
        private float t = 24f;
        private float currentAphla = 1;
        private bool fadeAnimate;
        public bool fadeStart;
        private DialogModal modal;
        //private string[] missionName = {"尋找逆走鐘","失控的逆走鐘","梅花鹿","金絲猴","鯨豚", "早坂犀牛", "臺灣猛獁象", "臺灣長吻鱷" };

        public void StarIntroView()
        {
            iBeaconMissionSetting.Instance.isEnterGame = true;
            PlayerPrefs.SetInt("guide", 1);
            ShowPanel(introView, true);

            modal = GameModals.instance.OpenModal<DialogModal>();
            modal.ShowIntro();
            modal.IntroEndAction += GuideUIView;
        }

        public void StarMainView()
        {
            SoundPlayerController.Instance.PlayBackgroundMusic();
            iBeaconMissionSetting.Instance.isEnterGame = false;
            MainButtonClick();
            MissionsButtonClick();
        }

        public void GetBackAnimal(int index)
        {
            Debug.Log("get back");
            
            buttonIndex = index - 2;

            if (buttonIndex >= 0 && buttonIndex < animalButtons.Length)
            {
                clockOutSide++;
                animalButtons[buttonIndex].GetComponent<Button>().interactable = true;

                ChangeClock();
            } 

            if (index == 8)
                Mission8.backToDinosaur = true;     
        }

        public void ShowFinishView(int index)
        {
            if (index == 8) return;

            var s = string.Format("恭喜您完成{0}關卡！\n 再去園區尋找下一個關卡吧！", StringAsset.Finish.missionName[index]);
            finishText.text = s;
            t = 10;
            ShowPanel(finishView, true);
        }       
        
        private void MainButtonClick()
        {
            for (int i = 0; i < animalButtons.Length; i++)
            {
                int closureIndex = i;
                animalButtons[closureIndex].onClick.AddListener(() =>
                {
                    var modal = Modals.instance.OpenModal<InfoModal>();
                    modal.ShowInfo(closureIndex, TypeFlag.InfoType.Animal);
                });
            }

            dinosaurButton.onClick.AddListener(() => {

                if(Mission8.backToDinosaur)
                {
                    var modal = Modals.instance.OpenModal<DinosaurlModal>();
                    modal.BackToDinosaurl(true);
                    modal.Setup();

                    Games.instance.GetGame<Game8>().CloseFoods();
                }
                else
                {
                    var modal = Modals.instance.OpenModal<DinosaurlModal>();
                    modal.BackToDinosaurl(false);
                }
            });

            eggButton.onClick.AddListener(() => {
                Modals.instance.OpenModal<EggModal>();
            });

            finishCloseButton.onClick.AddListener(() => {
                ShowPanel(finishView, false);
            });

            promptButton.onClick.AddListener(() => {
                ShowPanel(promptView, true);
                iBeaconMissionSetting.Instance.isEnterGame = true;
            });

            promptCloseButton.onClick.AddListener(() => {
                ShowPanel(promptView, false);
                iBeaconMissionSetting.Instance.isEnterGame = false;
            });

            for(int i = 0; i < remiindButtons.Length; i++)
            {
                var index = i;
                remiindButtons[index].onClick.AddListener(() => {
                    ShowPanel(finishView, true);
                    finishText.text = StringAsset.Remind.location[index];
                });
            }
        }

        private void MissionsButtonClick()
        {
            for (int i = 0; i < missionButtons.Length; i++)
            {
                int closureIndex = i;
                missionButtons[closureIndex].onClick.AddListener(() =>
                {
                    GameMissions.instance.ShowMission(closureIndex);
                });
            }
        }

        private void ChangeClock()
        {
            Debug.Log("clockOutSide " + clockOutSide);
            if (clockOutSide == 0) { clockImage.sprite = clockSprite[0]; }
            if (clockOutSide == 2) { clockImage.sprite = clockSprite[1]; }
            if (clockOutSide == 4) { clockImage.sprite = clockSprite[2]; }
            if (clockOutSide == 6)
            {
                var eggModal = Modals.instance.GetModel<EggModal>();
                clockImage.sprite = clockSprite[3];
                iBeaconMissionSetting.Instance.isLastMissionOpen = true;  // Finish all mission will open

                if (PlayerPrefs.HasKey("dinosaurBaby"))
                {
                    eggModal.SetButtonStatus();
                }
                else
                {
                    eggModal.StartButton.interactable = true;
                    eggModal.erroObject.SetActive(false);
                    remindObject.SetActive(true); // Finish all mission will open
                }                    
            }
        }

        private void GuideUIView()
        {
            modal.IntroEndAction -= GuideUIView;
            introImage.enabled = false;
            foreach (var b in guideView.gameObjects) { b.SetActive(false); }
            foreach (var b in guideView.gameObjectImages) { b.SetActive(false); }

            var i = 0;
            ShowPanel(guideView.canvasGroup, true);
            guideView.gameObjects[i].SetActive(true);
            guideView.gameObjectImages[i].SetActive(true);
            guideView.gameObjects[i].GetComponentInChildren<Text>().text = MainApp.Instance.guideData.m_Data[0].gamePrompt[i];

            guideView.button.onClick.AddListener(()=>
            {
                foreach (var b in guideView.gameObjects) { b.SetActive(false); }
                foreach (var b in guideView.gameObjectImages) { b.SetActive(false); }

                if ( i != guideView.gameObjects.Length - 1)
                {
                    i++;
                    guideView.gameObjects[i].SetActive(true);
                    guideView.gameObjectImages[i].SetActive(true);
                    guideView.gameObjects[i].GetComponentInChildren<Text>().text = MainApp.Instance.guideData.m_Data[0].gamePrompt[i];
                    Debug.Log("i: " + i);

                    if(i == guideView.gameObjects.Length - 1)
                    {
                        guideView.button.onClick.AddListener(() =>
                        {
                            ShowPanel(guideView.canvasGroup, false);
                            ShowPanel(introView, false);

                            StarMainView();
                        });
                    }
                }
            });
        }

        public void ShowPanel(CanvasGroup canvasGroup, bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;
            }
        }

        private void Update()
        {
            if (!fadeStart) return;
            if (buttonIndex < 0) return;
            if (buttonIndex == 7) return;

            t -= Time.deltaTime;

            if(t > 0)
            {
                if (animalButtons[buttonIndex].gameObject.GetComponent<Image>().color.a < 0.41f)
                    fadeAnimate = false;
                else if (animalButtons[buttonIndex].gameObject.GetComponent<Image>().color.a > 0.99f)
                    fadeAnimate = true;

                FadeImage(fadeAnimate);
            }
            else
            {
                animalButtons[buttonIndex].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                fadeStart = false;
            }
        }

        private void FadeImage(bool isFadeOut)
        {
            if(isFadeOut)
                currentAphla -= 0.3f * Time.deltaTime;
            else
                currentAphla += 0.3f * Time.deltaTime;

            animalButtons[buttonIndex].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, currentAphla);
        }
    }
}

[System.Serializable]
public class GuideView
{
    public CanvasGroup canvasGroup;
    public GameObject[] gameObjects; // guideImage1, guideImage3, guideImage2
    public GameObject[] gameObjectImages;
    public Button button;
}