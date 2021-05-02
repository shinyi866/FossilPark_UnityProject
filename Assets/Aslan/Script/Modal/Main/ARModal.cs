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
        private CanvasGroup picturePanel;

        [SerializeField]
        private Button ExitButton;

        [SerializeField]
        private GameObject image;

        [SerializeField]
        private GameObject backGameObject;

        [SerializeField]
        private GameObject pictureGameObject;

        [SerializeField]
        private GameObject mainSaveButton;

        [SerializeField]
        private GameObject gameSaveButton;

        public GameObject feedButtonGameObject;

        private Texture2D currentImage;
        private Button PictureButton;

        [SerializeField]
        private ARPlaneManager planeManager;

        // switch game AR view and main AR view
        public void ShowView(bool isMainView)
        {
            backGameObject.SetActive(isMainView);
            mainSaveButton.SetActive(isMainView);

            //imageGameObject.SetActive(!isMainView);
            gameSaveButton.SetActive(!isMainView);
        }

        private void Awake()
        {
            PictureButton = pictureGameObject.GetComponent<Button>();
            PictureButton.onClick.AddListener(() => { TakePicture(); });

            BackButton.onClick.AddListener(() =>
            {
                Modals.instance.CloseARInMain(); // TODO error?
                feedButtonGameObject.SetActive(false);
            });

            mainSaveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                //Modals.instance.CloseModal();
                //GameModals.instance.CloseModal();
                //Games.instance.ClosGame();
                SavePhotoPanel(false);
                ClosePicturePanel(false);
                Modals.instance.CloseARInMain();
                iBeaconMissionSetting.Instance.isEnterGame = false; // start detect ibeacon???
                CloseARPlane();
                //SaveImage();
            });

            gameSaveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowView(true);
                SavePhotoPanel(false);
                ClosePicturePanel(false);
                Games.instance.ClosGame();
                Modals.instance.CloseARInGame();

                iBeaconMissionSetting.Instance.isEnterGame = false; // start detect ibeacon
                CloseARPlane();
                //Modals.instance.CloseModal();
                //GameModals.instance.CloseModal();

                //SaveImage();
                // TODO play video
            });

            ExitButton.onClick.AddListener(() => { SavePhotoPanel(false); });
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

            _camera.targetTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0); // (222, 128, 0);

            RenderTexture renderTexture = _camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            _camera.Render();
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();

            Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
            image.GetComponent<Image>().sprite = screenShot;
            currentImage = renderResult;
            SavePhotoPanel(true);

            _camera.targetTexture = null;
        }

        private void SavePhotoPanel(bool isShow)
        {
            if (picturePanel != null)
            {
                picturePanel.alpha = (isShow) ? 1 : 0;
                picturePanel.interactable = isShow;
                picturePanel.blocksRaycasts = isShow;

                pictureGameObject.SetActive(!isShow);
            }
        }

        private void ClosePicturePanel(bool isShow)
        {
            this.canvasGroup.alpha = (isShow) ? 1 : 0;
            this.canvasGroup.interactable = isShow;
            this.canvasGroup.blocksRaycasts = isShow;
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