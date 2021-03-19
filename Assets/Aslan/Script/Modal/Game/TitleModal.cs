using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TitleModal : Modal
    {
        [Header("Text")]
        [SerializeField]
        private Text messageText;

        [SerializeField]
        private Text titleText;

        [Header("Buttons")]
        [SerializeField]
        private GameObject[] buttonObj; // 0:CloseButton, 1:ConfirmButton

        [Header("Image")]
        [SerializeField]
        private Image image;

        private Button CloseButton;
        [HideInInspector]
        public Button ConfirmButton;

        private GameDialogData data;

        private void Awake()
        {
            data = MainApp.Instance.database;

            CloseButton = buttonObj[0].GetComponent<Button>();
            ConfirmButton = buttonObj[1].GetComponent<Button>();
            CloseButton.onClick.AddListener(() => { GameModals.instance.CloseModal(); });
        }

        public void ShowInfo(int index, TypeFlag.TitleType type)
        {
            var gameData = data.m_Data[index];
            foreach (var g in buttonObj) { g.SetActive(false); }
            ConfirmButton.onClick.RemoveAllListeners();

            switch (type)
            {
                case TypeFlag.TitleType.RoundTitleNotify:
                    titleText.text = gameData.roundMessage[0];
                    messageText.text = gameData.roundMessage[1];
                    buttonObj[0].SetActive(true);
                    image.sprite = gameData.animalNotifyPicture;
                    Debug.Log("RoundTitleNotify ");
                    break;

                case TypeFlag.TitleType.EnterTitle:
                    titleText.text = gameData.roundMessage[0];
                    messageText.text = gameData.enterMessage;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.enterButton;
                    buttonObj[1].SetActive(true);
                    image.sprite = gameData.animalNotifyPicture;
                    Debug.Log("EnterNotify ");
                    break;
                    
                case TypeFlag.TitleType.GameTitle:
                    titleText.text = gameData.gameTitle[0];
                    messageText.text = gameData.gameTitle[1];
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.titleButton;
                    //===notifyText.text = gameData.gameNotify[0];
                    buttonObj[1].SetActive(true);
                    Debug.Log("TitleModal ");
                    break;
            }
        }
    }
}
