using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class DialogModal : Modal
    {
        [SerializeField]
        private Text title;
        [SerializeField]
        private Text message;
        [SerializeField]
        private Image image;
        [SerializeField]
        public Button ConfirmButton;

        private GameDialogData data;

        private void Awake()
        {
            data = MainApp.Instance.database;
        }

        public void ShowInfo(int index, TypeFlag.DialogType type)
        {
            if (data == null) { Debug.Log("null"); }
            var gameData = data.m_Data[index];
            ConfirmButton.onClick.RemoveAllListeners();

            switch (type)
            {
                case TypeFlag.DialogType.EnterDialog:
                    title.text = "遊戲提示";
                    message.text = gameData.gameDescription;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.dialogButton;
                    break;
                case TypeFlag.DialogType.SuccessDialog:
                    title.text = "成功";
                    message.text = gameData.success;
                    ConfirmButton.GetComponentInChildren<Text>().text = "確定";
                    break;
                case TypeFlag.DialogType.FailDialog:
                    title.text = "失敗";
                    message.text = gameData.fail;
                    ConfirmButton.GetComponentInChildren<Text>().text = "確定";
                    break;
            }

            image.sprite = gameData.animalDialogPicture;
        }
    }
}
