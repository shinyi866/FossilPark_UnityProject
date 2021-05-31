using System;
using System.Collections;
using System.Collections.Generic;
using GameMission;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace View
{
    public class ARModal : Modal
    {
        [SerializeField]
        private Button BackButton;

        [SerializeField]
        private PicturePanel picturePanel;

        [SerializeField]
        private LeavePanel leavePanel;

        [SerializeField]
        private GameObject backGameObject;

        [SerializeField]
        private GameObject pictureGameObject;

        [SerializeField]
        private GameObject gameSaveButton;

        public GameObject feedButtonGameObject;
        public Image feedImage;
        public Button feedButton;

        private Texture2D currentImage;
        private Button PictureButton;        

        [SerializeField]
        private ARPlaneManager planeManager;

        // switch game AR view and main AR view
        public void ShowView(bool isMainView)
        {
            backGameObject.SetActive(isMainView);
            picturePanel.mainSaveButton.SetActive(isMainView);

            //imageGameObject.SetActive(!isMainView);
            gameSaveButton.SetActive(!isMainView);
        }

        private void Awake()
        {
            PictureButton = pictureGameObject.GetComponent<Button>();
            PictureButton.onClick.AddListener(() => { TakePicture(); });

            BackButton.onClick.AddListener(() => { ShowPanel(leavePanel.canvasGroup, true); });

            leavePanel.button_confirm.onClick.AddListener(()=>
            {
                feedButtonGameObject.SetActive(false);
                ShowPanel(leavePanel.canvasGroup, false);
                ShowPanel(picturePanel.canvasGroup, false);

                //Games.instance.OpenGame<Game8>().DestoryFoods();
                Games.instance.ClosGame();
                GameModals.instance.GetModal<ARGameModal>().CloseAllPanel();
                GameModals.instance.CloseModal();
                Modals.instance.CloseModal();
                Modals.instance.CloseARInMain();
                Modals.instance.CloseARInGame();
                MediaPlayerController.instance.StopVideo();
                iBeaconMissionSetting.Instance.isEnterGame = false; // start detect ibeacon
                CameraCtrl.instance.DisableOcclusionManager();
                
                CloseARPlane();
            });

            leavePanel.button_cancle.onClick.AddListener(() => { ShowPanel(leavePanel.canvasGroup, false); });

            picturePanel.mainSaveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                /*
                ClosePicturePanel(false);
                Modals.instance.CloseARInMain();
                iBeaconMissionSetting.Instance.isEnterGame = false; // start detect ibeacon???
                CloseARPlane();
                */
                ShowPanel(picturePanel.canvasGroup, false);
                SaveImage();
            });

            gameSaveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowView(true);
                ShowPanel(picturePanel.canvasGroup, false);
                /*
                ClosePicturePanel(false);
                Games.instance.ClosGame();
                Modals.instance.CloseARInGame();

                iBeaconMissionSetting.Instance.isEnterGame = false; // start detect ibeacon
                CloseARPlane();
                */
                SaveImage();
            });

            picturePanel.button_exit.onClick.AddListener(() => { ShowPanel(picturePanel.canvasGroup, false); });
        }

        private void CloseARPlane()
        {
            planeManager.enabled = false;

            foreach(ARPlane aRPlane in planeManager.trackables)
            {
                aRPlane.gameObject.SetActive(false);
            }
        }

        private void TakePicture()
        {
            StartCoroutine(RenderScreenShot());
        }


        private IEnumerator RenderScreenShot()
        {
            Camera _camera = CameraCtrl.instance.GetCurrentCamera();

            yield return new WaitForSeconds(0.1f);

            _camera.targetTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 1);  // (440, 250, 1); // (_camera.pixelWidth, _camera.pixelHeight, 1);

            RenderTexture renderTexture = _camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            _camera.Render();
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();

            Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
            picturePanel.image.GetComponent<Image>().sprite = screenShot;
            currentImage = renderResult;
            //SavePhotoPanel(true);
            ShowPanel(picturePanel.canvasGroup, true);

            _camera.targetTexture = null;
        }

        public void ShowPanel(CanvasGroup canvasGroup, bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;

                if (canvasGroup == picturePanel.canvasGroup)
                    pictureGameObject.SetActive(!isShow);
            }
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
public class PicturePanel
{
    public CanvasGroup canvasGroup;
    public Button button_exit;
    public Image image;
    public GameObject mainSaveButton;
    public GameObject gameSaveButton;
}

[System.Serializable]
public class LeavePanel
{
    public CanvasGroup canvasGroup;
    public Button button_cancle;
    public Button button_confirm;
}