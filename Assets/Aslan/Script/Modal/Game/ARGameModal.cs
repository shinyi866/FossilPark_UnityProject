using System.Collections;
using System.Collections.Generic;
using GameMission;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace View
{
    public class ARGameModal : Modal
    {
        [SerializeField]
        private ARPlaneManager planeManager;
        
        private Mission5 mission5;
        private Mission6 mission6;
        private Mission7 mission7;

        public Text text;
        public Button backButton;

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
            mission5 = GameMissions.instance.GetMission<Mission5>();
            mission6 = GameMissions.instance.GetMission<Mission6>();
            mission7 = GameMissions.instance.GetMission<Mission7>();

            SwitchConfirmButton(false); //default button
            gamePromptPanel.buttonsObject.SetActive(false); //default button

            gamePromptPanel.button.onClick.AddListener(() => {
                ShowPanel(gamePromptPanel.canvasGroup, false);
                gamePromptPanel.image.sprite = null;
            });

            // save picture
            gamePromptPanel.button_save.onClick.AddListener(() => {
                SaveImage();

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

            backButton.onClick.AddListener(() =>
            {
                Games.instance.ClosGame();
                Games.instance.StopGame();
                GameModals.instance.GetModal<ARGameModal>().CloseAllPanel();
                GameModals.instance.CloseModal();
                Modals.instance.CloseModal();
                Modals.instance.CloseARInMain();
                Modals.instance.CloseARInGame();
                MediaPlayerController.instance.DestroyVideo();
                iBeaconMissionSetting.Instance.IbeaconNotDetect(false);
                CameraCtrl.instance.DisableOcclusionManager();
                mission5.BackToMain();
                mission6.BackToMain();
                mission7.BackToMain();
                ShowPanel(gamePromptPanel.canvasGroup, false);
                CloseARPlane();
            });
        }

        private void CloseARPlane()
        {
            planeManager.enabled = false;

            foreach (ARPlane aRPlane in planeManager.trackables)
            {
                aRPlane.gameObject.SetActive(false);
            }
        }

        public void ShowModal(int index, TypeFlag.ARGameType type)
        {
            var supportAR = MainApp.Instance.isARsupport;
            var gameData = data.m_Data[index];

            CloseAllPanel();

            backButton.image.enabled = true;
            text.text = gameData.gameNotify[0];
            SwitchConfirmButton(false);


            switch (type)
            {
                case TypeFlag.ARGameType.Game0:
                    ShowPanel(game0Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game1:
                    text.text = gameData.gamePrompt[1];
                    gamePromptPanel.image.sprite = gameData.endPicutre;
                    break;
                case TypeFlag.ARGameType.Game2:
                    ShowPanel(game2Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game3:
                    ShowPanel(game3Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game6:
                    ShowPanel(game6Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game8:
                    ShowPanel(game8Panel.canvasGroup, true);
                    CloseBackButton(true);
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
                    
                    if (gameData.gamePrompt.Length > 3)
                    {
                        text.text = gameData.gameNotify[0];
                        gamePromptPanel.image.sprite = gameData.guidePicture2;
                    }
                    break;
                case TypeFlag.ARGameType.GamePrompt2:
                    gamePromptPanel.text.text = gameData.gamePrompt[1];
                    ShowPanel(gamePromptPanel.canvasGroup, true);

                    if (gameData.gamePrompt.Length > 4)
                    {
                        text.text = gameData.gameNotify[1];
                        gamePromptPanel.image.sprite = gameData.endPicutre;
                    }
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
            Debug.Log("switch!!!!  " + switchToConfirm);
            gamePromptPanel.button.gameObject.SetActive(!switchToConfirm);
            gamePromptPanel.button_confirm.gameObject.SetActive(switchToConfirm);
        }
        public void CloseBackButton(bool isClose)
        {
            backButton.image.enabled = !isClose;
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
            StartCoroutine(RenderScreenShot());
        }

        private IEnumerator RenderScreenShot()
        {
            Camera _camera = CameraCtrl.instance.GetCurrentCamera();
            yield return new WaitForEndOfFrame();  //WaitForSeconds(0.1f);
            Debug.Log("yoooo");
            _camera.targetTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0); // (222, 128, 0);

            RenderTexture renderTexture = _camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            _camera.Render();
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();

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
    public Button button_go;
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
    //public Button pictureButton;
    //public Button backButton;
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
