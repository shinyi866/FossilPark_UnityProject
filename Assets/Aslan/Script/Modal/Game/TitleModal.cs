using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TitleModal : Modal
    {
        [SerializeField]
        private RoundTitlePanel roundTitlePanel;
        [SerializeField]
        private EnterTitlePanel enterTitlePanel;
        [SerializeField]
        private EnterGame0TitlePanel enterGame0TitlePanel;
        [SerializeField]
        private Game01TitlePanel game01TitlePanel;

        [Header("Text")]
        [SerializeField]
        private Text titleText;

        [HideInInspector]
        public Button ConfirmButton;
        [HideInInspector]
        public Button Game0ConfirmButton;

        private CanvasGroup[] GameCanvasGroups;
        private GameDialogData data;

        private void Awake()
        {
            data = MainApp.Instance.database;

            ConfirmButton = enterTitlePanel.button;
            Game0ConfirmButton = enterGame0TitlePanel.button;
            roundTitlePanel.button.onClick.AddListener(() => { GameModals.instance.CloseModal(); });

            GameCanvasGroups = new CanvasGroup[] { roundTitlePanel.canvasGroup, enterTitlePanel.canvasGroup, enterGame0TitlePanel.canvasGroup, game01TitlePanel.canvasGroup };
        }

        public void ShowInfo(int index, TypeFlag.TitleType type)
        {
            var gameData = data.m_Data[index];

            foreach (var c in GameCanvasGroups) { ShowPanel(c, false); }
            ConfirmButton.onClick.RemoveAllListeners();

            switch (type)
            {
                case TypeFlag.TitleType.RoundTitleNotify:
                    ShowPanel(roundTitlePanel.canvasGroup, true);
                    titleText.text = gameData.roundMessage[0];
                    roundTitlePanel.text.text = gameData.roundMessage[1];
                    roundTitlePanel.image.sprite = gameData.animalNotifyPicture;
                    Debug.Log("RoundTitleNotify ");
                    break;

                case TypeFlag.TitleType.EnterTitle:
                    ShowPanel(enterTitlePanel.canvasGroup, true);
                    titleText.text = gameData.roundMessage[0];
                    enterTitlePanel.text.text = gameData.enterMessage;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.enterButton;
                    enterTitlePanel.image.sprite = gameData.animalNotifyPicture;
                    Debug.Log("EnterNotify ");
                    break;
                    
                case TypeFlag.TitleType.GameTitle:
                    ShowPanel(enterTitlePanel.canvasGroup, true);
                    titleText.text = gameData.gameTitle[0];
                    enterTitlePanel.text.text = gameData.gameTitle[1];
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.titleButton;
                    //===notifyText.text = gameData.gameNotify[0];
                    Debug.Log("TitleModal ");
                    break;
                case TypeFlag.TitleType.EnterGame0:
                    ShowPanel(enterGame0TitlePanel.canvasGroup, true);
                    titleText.text = gameData.gameTitle[0];
                    enterGame0TitlePanel.text.text = gameData.enterMessage;
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.enterButton;
                    break;
            }
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
public class RoundTitlePanel
{
    public CanvasGroup canvasGroup;
    public Text text;
    public Image image;
    public Button button;
}

[System.Serializable]
public class EnterTitlePanel
{
    public CanvasGroup canvasGroup;
    public Text text;
    public Image image;
    public Button button;
}

[System.Serializable]
public class EnterGame0TitlePanel
{
    public CanvasGroup canvasGroup;
    public Image image;
    public Text text;
    public Button button;
}

[System.Serializable]
public class Game01TitlePanel
{
    public CanvasGroup canvasGroup;
    public Image image1;
    public Image image2;
    public Image image3;
    public Button button;
}