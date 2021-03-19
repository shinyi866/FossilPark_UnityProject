using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class DialogModal : Modal
    {
        [SerializeField]
        private Text message;

        [SerializeField]
        private Image image;      
        
        [Header("Buttons")]
        [SerializeField]
        private GameObject[] buttonObj; // 0:NextButton, 1:ConfirmButton

        [HideInInspector]
        public Button ConfirmButton;
        [HideInInspector]
        public Button NextButton;

        private GameDialogData data;

        private void Awake()
        {
            data = MainApp.Instance.database;

            NextButton = buttonObj[0].GetComponent<Button>();
            ConfirmButton = buttonObj[1].GetComponent<Button>();
        }

        public void ShowInfo(int index, TypeFlag.DialogType type)
        {
            if (data == null) { Debug.Log("null"); }

            var gameData = data.m_Data[index];
            foreach (var g in buttonObj)
            {
                g.SetActive(false);
                g.GetComponent<Button>().onClick.RemoveAllListeners();
            }

            switch (type)
            {
                case TypeFlag.DialogType.EnterDialog:
                    message.text = gameData.gameDescription;
                    buttonObj[0].SetActive(true);
                    //===ConfirmButton.GetComponentInChildren<Text>().text = gameData.dialogButton;
                    break;
                case TypeFlag.DialogType.EndDialog:
                    buttonObj[1].SetActive(true);
                    message.text = gameData.endMessage;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.pictureNotify;
                    break;
                case TypeFlag.DialogType.FailDialog:
                    //===message.text = gameData.fail;
                    ConfirmButton.GetComponentInChildren<Text>().text = "確定";
                    break;
            }

            image.sprite = gameData.animalDialogPicture;
        }
    }
}
