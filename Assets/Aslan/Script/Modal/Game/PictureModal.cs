using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

namespace View
{
    public class PictureModal : Modal
    {
        [SerializeField]
        private ARpicturePanel arPicturePanel;
        [SerializeField]
        private OnePictureGuidePanel onePictureGuidePanel;
        [SerializeField]
        private TwoPictureGuidePanel twoPictureGuidePanel;

        [HideInInspector]
        public Button ConfirmButton;
        [HideInInspector]
        public Button GuideConfirmButtonOne;
        [HideInInspector]
        public Button GuideConfirmButtonTwo;

        [HideInInspector]
        public Button SaveButton;
        [HideInInspector]
        public Button ExitButton;
        [HideInInspector]
        public Image ARimage;

        private Image _image;
        private GameDialogData data;
        private TypeFlag.PictureType currentType;
        private CanvasGroup[] GameCanvasGroups;

        private void Awake()
        {
            data = MainApp.Instance.database;

            GuideConfirmButtonOne = onePictureGuidePanel.button;
            GuideConfirmButtonTwo = twoPictureGuidePanel.button;
            SaveButton = arPicturePanel.saveButton;
            ExitButton = arPicturePanel.exitButton;
            ARimage = arPicturePanel.arImage;

            GameCanvasGroups = new CanvasGroup[] { arPicturePanel.canvasGroup, onePictureGuidePanel.canvasGroup, twoPictureGuidePanel.canvasGroup};

            //===PictureButton.onClick.AddListener(() => { TakePicture(); });

            SaveButton.onClick.AddListener(() =>
            {
                // TODO save picture to phone
            });

            ExitButton.onClick.AddListener(() => { ShowPanel(arPicturePanel.canvasGroup, false); });

            ConfirmButton.onClick.AddListener(() =>
            {
                //===ShowPhotoPanel(photoButtonPanel, true);
                ShowPanel(onePictureGuidePanel.canvasGroup, false);
            });
        }

        private void Init()
        {
            foreach (var c in GameCanvasGroups) { ShowPanel(c, false); }

            GuideConfirmButtonOne.onClick.RemoveAllListeners();
            GuideConfirmButtonTwo.onClick.RemoveAllListeners();
        }

        public void ResetView()
        {
            //PictureButton.interactable = true;
            ShowPanel(arPicturePanel.canvasGroup, false);
            ShowPanel(onePictureGuidePanel.canvasGroup, false);
        }

        public void ShowInfo(int index, TypeFlag.PictureType type)
        {
            var gameData = data.m_Data[index];

            Init();
            currentType = type;

            switch (type)
            {
                case TypeFlag.PictureType.ARtype:
                    _image = ARimage;
                    //message.text = gameData.pictureNotify;
                    break;
                case TypeFlag.PictureType.GuideType:
                    var id = gameData.guideID;

                    if (id == 0)
                    {
                        ShowPanel(onePictureGuidePanel.canvasGroup, true);
                        onePictureGuidePanel.text.text = gameData.gameGuide[0];
                        onePictureGuidePanel.image.sprite = gameData.guidePicture2;
                    }
                    else if (id == 1)
                    {
                        ShowPanel(onePictureGuidePanel.canvasGroup, true);
                        onePictureGuidePanel.text.text = gameData.gameGuide[0];
                        onePictureGuidePanel.image.sprite = gameData.guidePicture1;
                    }
                    else if (id == 2)
                    {
                        ShowPanel(twoPictureGuidePanel.canvasGroup, true);
                        twoPictureGuidePanel.text.text = gameData.gameGuide[0];
                        twoPictureGuidePanel.leftText.text = gameData.gameGuide[1];
                        twoPictureGuidePanel.rightText.text = gameData.gameGuide[2];
                        twoPictureGuidePanel.leftImage.sprite = gameData.guidePicture1;
                        twoPictureGuidePanel.rightImage.sprite = gameData.guidePicture2;
                    }
                    else
                    {
                        Debug.Log("None guide ID");
                    }
                    
                    break;
                case TypeFlag.PictureType.EndGuide:
                    ShowPanel(twoPictureGuidePanel.canvasGroup, true);
                    twoPictureGuidePanel.text.text = gameData.endMessage[0];
                    twoPictureGuidePanel.leftText.text = gameData.endMessage[1];
                    twoPictureGuidePanel.rightText.text = gameData.endMessage[2];
                    twoPictureGuidePanel.leftImage.sprite = gameData.endPicutre;
                    twoPictureGuidePanel.rightImage.sprite = gameData.endPicutre2;
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
public class OnePictureGuidePanel
{
    public CanvasGroup canvasGroup;
    public Text text;
    public Image image;
    public Button button;
}

[System.Serializable]
public class TwoPictureGuidePanel
{
    public CanvasGroup canvasGroup;
    public Text text;
    public Text leftText;
    public Text rightText;
    public Image leftImage;
    public Image rightImage;
    public Button button;
}

[System.Serializable]
public class ARpicturePanel
{
    public CanvasGroup canvasGroup;
    public Button saveButton;
    public Button exitButton;
    public Image arImage;
}