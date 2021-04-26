﻿using System.Collections;
using System.Collections.Generic;
using GameMission;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ARGameModal : Modal
    {
        public Text text;

        public GamePromptPanel gamePromptPanel;
        public Game0Panel game0Panel;
        public Game2Panel game2Panel;
        public Game3Panel game3Panel;
        public Game6Panel game6Panel;
        public Game8Panel game8Panel;

        private CanvasGroup[] GameCanvasGroups;
        private GameDialogData data;
        private Texture2D currentImage;

        private void Awake()
        {
            data = MainApp.Instance.database;
            GameCanvasGroups = new CanvasGroup[] { game0Panel.canvasGroup, game2Panel.canvasGroup, game3Panel.canvasGroup, game6Panel.canvasGroup, game8Panel.canvasGroup };

            SwitchConfirmButton(false); //default button
            gamePromptPanel.buttonsObject.SetActive(false); //default button

            gamePromptPanel.button.onClick.AddListener(() => {
                ShowPanel(gamePromptPanel.canvasGroup, false);
                gamePromptPanel.image.sprite = null;
            });

            // save picture
            gamePromptPanel.button_save.onClick.AddListener(() => {
                //SaveImage();

                gamePromptPanel.button.gameObject.SetActive(true); //default button
                gamePromptPanel.buttonsObject.SetActive(false); //default button
                ShowPanel(gamePromptPanel.canvasGroup, false);
                gamePromptPanel.image.sprite = null;
                currentImage = null;
            });

            // leave
            gamePromptPanel.button_leave.onClick.AddListener(() => {
                gamePromptPanel.button.gameObject.SetActive(true); //default button
                gamePromptPanel.buttonsObject.SetActive(false); //default button
                ShowPanel(gamePromptPanel.canvasGroup, false);
                gamePromptPanel.image.sprite = null;
                currentImage = null;
            });


            game8Panel.pictureButton.onClick.AddListener(() =>
            {
                TakePicture();
                ShowPrompt(8, TypeFlag.ARGameType.PicturePrompt);
            });

            game8Panel.backButton.onClick.AddListener(() => {
                Games.instance.ClosGame();
                GameModals.instance.CloseModal();
                ShowPanel(game8Panel.canvasGroup, false);
                Modals.instance.OpenModal<MainModal>();
                iBeaconMissionSetting.Instance.isEnterGame = false;
            });

        }

        public void ShowModal(int index, TypeFlag.ARGameType type)
        {
            var supportAR = MainApp.Instance.isARsupport;
            var gameData = data.m_Data[index];

            CloseAllPanel();

            text.text = gameData.gameNotify[0];
            SwitchConfirmButton(false);

            switch (type)
            {
                case TypeFlag.ARGameType.Game0:
                    ShowPanel(game0Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game2:
                    ShowPanel(game2Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game3:
                    //===if (!supportAR) { unSupportView.SetActive(true); }
                    ShowPanel(game3Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game6:
                    ShowPanel(game6Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game8:
                    ShowPanel(game8Panel.canvasGroup, true);
                    break;
                default:
                    text.text = gameData.gameNotify[0];
                    break;
            }
        }

        public void ShowPrompt(int index, TypeFlag.ARGameType type)
        {
            var gameData = data.m_Data[index];
            //CloseAllPanel();

            switch (type)
            {
                case TypeFlag.ARGameType.GamePrompt1:
                    gamePromptPanel.text.text = gameData.gamePrompt[0];
                    ShowPanel(gamePromptPanel.canvasGroup, true);
                    
                    if (gameData.gamePrompt.Length > 4)
                        text.text = gameData.gameNotify[0];
                    break;
                case TypeFlag.ARGameType.GamePrompt2:
                    gamePromptPanel.text.text = gameData.gamePrompt[1];
                    ShowPanel(gamePromptPanel.canvasGroup, true);

                    if (gameData.gamePrompt.Length > 4)
                        text.text = gameData.gameNotify[1];
                    break;
                case TypeFlag.ARGameType.GamePrompt3:
                    gamePromptPanel.text.text = gameData.gamePrompt[2];
                    ShowPanel(gamePromptPanel.canvasGroup, true);

                    if (gameData.gamePrompt.Length > 4)
                        text.text = gameData.gameNotify[2];
                    break;
                case TypeFlag.ARGameType.GamePrompt4:
                    gamePromptPanel.text.text = gameData.gamePrompt[3];
                    ShowPanel(gamePromptPanel.canvasGroup, true);

                    if (gameData.gamePrompt.Length > 4)
                        text.text = gameData.gameNotify[3];
                    break;
                case TypeFlag.ARGameType.GamePrompt5:
                    gamePromptPanel.text.text = gameData.gamePrompt[4];
                    ShowPanel(gamePromptPanel.canvasGroup, true);

                    if (gameData.gamePrompt.Length > 4)
                        text.text = gameData.gameNotify[4];
                    break;
                case TypeFlag.ARGameType.PicturePrompt:
                    gamePromptPanel.text.text = gameData.gamePrompt[0];
                    gamePromptPanel.button.gameObject.SetActive(false);
                    gamePromptPanel.buttonsObject.SetActive(true);
                    ShowPanel(gamePromptPanel.canvasGroup, true);
                    break;
            }
        }

        public void SwitchConfirmButton(bool switchToConfirm)
        {
            gamePromptPanel.button.gameObject.SetActive(!switchToConfirm);
            gamePromptPanel.button_confirm.gameObject.SetActive(switchToConfirm);
        }

        public void ShowPanel(CanvasGroup canvasGroup,bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;
            }
        }

        public void CloseAllPanel()
        {
            foreach (var c in GameCanvasGroups) { ShowPanel(c, false); }
        }

        public void TakePicture()
        {
            //StartCoroutine(StartPhoto());
            StartCoroutine(RenderScreenShot());
        }

        IEnumerator StartPhoto()
        {
            yield return new WaitForEndOfFrame();

            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();
            
            Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
            gamePromptPanel.image.sprite = screenShot;
            currentImage = renderResult;
        }

        private IEnumerator RenderScreenShot()
        {
            Camera _camera = CameraCtrl.instance.GetCurrentCamera();
            yield return new WaitForEndOfFrame();  //WaitForSeconds(0.1f);
            Debug.Log("yoooo");
            _camera.targetTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0); // (222, 128, 0);

            RenderTexture renderTexture = _camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);//TextureFormat.ARGB32
            _camera.Render();
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();
            //byte[] bytes = renderResult.EncodeToPNG();
            //string s = string.Format("{0}/good.png", Application.dataPath);
            //System.IO.File.WriteAllBytes(s, bytes);
            //Texture2D newTexture = new Texture2D(renderTexture.width, renderTexture.height);
            //newTexture.LoadImage(bytes);
            Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
            gamePromptPanel.image.sprite = screenShot;
            currentImage = renderResult;

            _camera.targetTexture = null;
        }

        private void SaveImage()
        {
            if (currentImage == null) return;

            // save in memory
            string filename = FileName();
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(currentImage, "FossilParkGallery", filename, (success, path) => Debug.Log("Media save result: " + success + " " + path));
        }

        private string FileName()
        {
            return string.Format("screen_{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
    }
}

[System.Serializable]
public class Game0Panel
{
    public CanvasGroup canvasGroup;
    public Button button;
}

[System.Serializable]
public class Game2Panel
{
    public CanvasGroup canvasGroup;
    public Image[] images;
    public Image[] findImages; // 0.deer 1.rhino 2.elephone
}

[System.Serializable]
public class Game3Panel
{
    public CanvasGroup canvasGroup;
    public Image image;
    public Text text;
    public GameObject unSupportView;
    public Button leftButton;
    public Button rightButton;
}

[System.Serializable]
public class Game6Panel
{
    public CanvasGroup canvasGroup;
    public Button button;
}

[System.Serializable]
public class Game8Panel
{
    public CanvasGroup canvasGroup;
    public Button[] foodButtons;
    public Button pictureButton;
    public Button backButton;
}

[System.Serializable]
public class GamePromptPanel
{
    public CanvasGroup canvasGroup;
    public Text text;
    public Image image;
    public GameObject buttonsObject;
    public Button button; // close modal
    public Button button_confirm; //other modal addListener
    public Button button_save; //picture save
    public Button button_leave; //picture leave
}
