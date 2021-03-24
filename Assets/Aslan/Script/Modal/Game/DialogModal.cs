using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class DialogModal : Modal
    {
        [SerializeField]
        private IntroDialogPanel introDialogPanel;
        [SerializeField]
        private EnterDialogPanel enterDialogPanel;
        [SerializeField]
        private EndDialogPanel endDialogPanel;

        [SerializeField]
        private Text message;
        [SerializeField]
        private Image image;

        [HideInInspector]
        public Button ConfirmButton;
        [HideInInspector]
        public Button NextButton;

        public System.Action IntroEndAction;

        private CanvasGroup[] GameCanvasGroups; 
        private Button[] buttons; // 0:NextButton, 1:ConfirmButton
        private GameDialogData data;
        private MainGuideData guideData;
        private int next = 0;
        private int nextCount;

        private void Awake()
        {
            data = MainApp.Instance.database;

            GameCanvasGroups = new CanvasGroup[] { enterDialogPanel.canvasGroup, endDialogPanel.canvasGroup };
            buttons = new Button[] { enterDialogPanel.Nextbutton, endDialogPanel.ConfirmButton };

            NextButton = enterDialogPanel.Nextbutton;
            ConfirmButton = endDialogPanel.ConfirmButton;

            introDialogPanel.Introbutton.onClick.AddListener(() =>
            {
                if (next != nextCount - 1)
                {
                    next++;
                    message.text = guideData.m_Data[0].introMessage[next];

                    if (next == nextCount - 1)
                    {
                        introDialogPanel.image.enabled = true;

                        introDialogPanel.Introbutton.onClick.AddListener(() =>
                        {
                            ShowPanel(introDialogPanel.canvasGroup, false);
                            introDialogPanel.image.enabled = false;
                            guideData = null;
                            GameModals.instance.CloseModal();
                            IntroEnd();
                        });
                    }
                }

            });
        }

        private void Init()
        {
            foreach (var c in GameCanvasGroups) { ShowPanel(c, false); }
            foreach (var b in buttons) { b.onClick.RemoveAllListeners(); }
        }

        public void ShowInfo(int index, TypeFlag.DialogType type)
        {
            if (data == null) { Debug.Log("null"); }
            var gameData = data.m_Data[index];

            Init();

            switch (type)
            {
                case TypeFlag.DialogType.EnterDialog:
                    ShowPanel(enterDialogPanel.canvasGroup, true);
                    message.text = gameData.gameDescription;
                    break;
                case TypeFlag.DialogType.EndDialog:
                    ShowPanel(endDialogPanel.canvasGroup, true);
                    message.text = gameData.endMessage[0];
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.pictureNotify;
                    break;
                case TypeFlag.DialogType.FailDialog:
                    ShowPanel(endDialogPanel.canvasGroup, true);
                    message.text = gameData.endMessage[1];
                    ConfirmButton.GetComponentInChildren<Text>().text = gameData.pictureNotify;
                    break;
            }

            image.sprite = gameData.animalDialogPicture;
        }

        public void ShowIntro()
        {
            guideData = MainApp.Instance.guideData;
            nextCount = guideData.m_Data[0].introMessage.Length;

            ShowPanel(introDialogPanel.canvasGroup, true);

            message.text = guideData.m_Data[0].introMessage[next];
            image.sprite = guideData.m_Data[0].guideAnimal;
        }

        private void IntroEnd()
        {
            if (IntroEndAction != null)
                IntroEndAction();
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
public class IntroDialogPanel
{
    public CanvasGroup canvasGroup;
    public Button Introbutton;
    public Image image;
}

[System.Serializable]
public class EnterDialogPanel
{
    public CanvasGroup canvasGroup;
    public Button Nextbutton;
}

[System.Serializable]
public class EndDialogPanel
{
    public CanvasGroup canvasGroup;
    public Button ConfirmButton;
}