using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class NotifyModal : Modal
    {
        [Header("Text")]
        [SerializeField]
        private Text notifyText;

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

        public void ShowInfo(int index, TypeFlag.NotifyType type)
        {
            var gameData = data.m_Data[index];
            foreach (var g in buttonObj) { g.SetActive(false); }
            ConfirmButton.onClick.RemoveAllListeners();

            switch (type)
            {
                case TypeFlag.NotifyType.RoundNotify:
                    notifyText.text = gameData.roundMessage;
                    buttonObj[0].SetActive(true);
                    Debug.Log("RoundNotify ");
                    break;

                case TypeFlag.NotifyType.EnterNotify:
                    notifyText.text = gameData.enterMessage;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.enterButton;
                    buttonObj[1].SetActive(true);
                    image.sprite = gameData.animalNotifyPicture;
                    Debug.Log("EnterNotify ");
                    break;

                case TypeFlag.NotifyType.GameNotify:
                    notifyText.text = gameData.gameNotify[0];
                    buttonObj[1].SetActive(true);
                    Debug.Log("GameNotify ");
                    break;
            }
        }
    }
}
