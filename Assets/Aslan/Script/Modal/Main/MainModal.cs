﻿using System.Collections;
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
        [SerializeField]
        private Image introImage;
        [SerializeField]
        private GuideView guideView;

        [Header("Buttons")]
        [SerializeField]
        private Button[] animalButtons;
        [SerializeField]
        private Button dinosaurButton;
        [SerializeField]
        private Button eggButton;

        [SerializeField]
        private Button[] missionButtons;

        [Header("Image")]
        public Image scoreImage;
        [SerializeField]
        private Image clockImage;
        [SerializeField]
        private Sprite[] clockSprite;

        private int clockOutSide = 0;
        private DialogModal modal;

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
            iBeaconMissionSetting.Instance.isEnterGame = false;
            MainButtonClick();
            MissionsButtonClick();
        }

        public void GetBackAnimal(int index)
        {
            Debug.Log("get back");
            var buttonIndex = index - 2;
            clockOutSide++;

            if (buttonIndex >= 0 && buttonIndex < animalButtons.Length)
            {
                animalButtons[buttonIndex].GetComponent<Image>().enabled = true;
                ChangeClock();
            }
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
                var modal = Modals.instance.OpenModal<DinosaurlModal>();
                modal.Setup();
            });

            eggButton.onClick.AddListener(() => {
                Modals.instance.OpenModal<EggModal>();
            });
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
            if (clockOutSide == 6) { clockImage.sprite = clockSprite[3]; }
        }

        private void GuideUIView()
        {
            modal.IntroEndAction -= GuideUIView;
            introImage.enabled = false;
            foreach (var b in guideView.gameObjects) { b.SetActive(false); }

            var i = 0;
            ShowPanel(guideView.canvasGroup, true);
            guideView.gameObjects[i].SetActive(true);
            guideView.gameObjects[i].GetComponentInChildren<Text>().text = MainApp.Instance.guideData.m_Data[0].gamePrompt[i];

            guideView.button.onClick.AddListener(()=>
            {
                foreach (var b in guideView.gameObjects) { b.SetActive(false); }

                if( i != guideView.gameObjects.Length - 1)
                {
                    i++;
                    guideView.gameObjects[i].SetActive(true);
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

        private void ShowPanel(CanvasGroup canvasGroup, bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;
            }
        }
    }
}

[System.Serializable]
public class GuideView
{
    public CanvasGroup canvasGroup;
    public GameObject[] gameObjects; // guideImage1, guideImage2, guideImage3
    public Button button;
}