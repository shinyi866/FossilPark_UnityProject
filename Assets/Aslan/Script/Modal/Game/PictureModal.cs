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
        private CanvasGroup ARpicturePanel;
        [SerializeField]
        private CanvasGroup findAnimalPanel;
        [SerializeField]
        private CanvasGroup photoButtonPanel;

        [SerializeField]
        private Button SaveButton;
        [SerializeField]
        private Button ExitButton;

        public Button ConfirmButton;
        public Button PictureButton;

        [SerializeField]
        private Image ARimage;
        [SerializeField]
        private Image missionImage;

        [SerializeField]
        private Text message;
        [SerializeField]
        private Text missionMessage;

        private Image _image;
        private GameDialogData data;
        private TypeFlag.PictureType currentType;

        private void Awake()
        {
            data = MainApp.Instance.database;

            //PictureButton.onClick.AddListener(() => { TakePicture(); });

            SaveButton.onClick.AddListener(() =>
            {
                // TODO save picture to phone
            });

            //ExitButton.onClick.AddListener(() => { SavePhotoPanel(false); });
            ConfirmButton.onClick.AddListener(() =>
            {
                ShowPhotoPanel(photoButtonPanel, true);
                ShowPhotoPanel(findAnimalPanel, false);
            });
        }

        public void ResetView()
        {
            PictureButton.interactable = true;
            ShowPhotoPanel(ARpicturePanel, false);
            ShowPhotoPanel(findAnimalPanel, false);
        }

        public void ShowInfo(int index, TypeFlag.PictureType type)
        {
            var gameData = data.m_Data[index];

            ShowPhotoPanel(ARpicturePanel, false);
            ShowPhotoPanel(findAnimalPanel, false);
            
            currentType = type;

            switch (type)
            {
                case TypeFlag.PictureType.ARtype:
                    _image = ARimage;
                    message.text = gameData.pictureNotify;
                    break;
                case TypeFlag.PictureType.MissionType:
                    ShowPhotoPanel(photoButtonPanel, true); // open photo button
                    _image = missionImage;
                    message.text = gameData.gameNotify[0];
                    missionMessage.text = gameData.gameNotify[0];
                    break;
                case TypeFlag.PictureType.SuccessCatch1:
                    missionMessage.text = gameData.gameNotify[1];
                    break;
                case TypeFlag.PictureType.SuccessCatch2:
                    missionMessage.text = gameData.gameNotify[2];
                    break;
                case TypeFlag.PictureType.SuccessCatch3:
                    missionMessage.text = gameData.gameNotify[3];
                    break;
                case TypeFlag.PictureType.HasCatch:
                    missionMessage.text = gameData.gameNotify[4];
                    break;
                case TypeFlag.PictureType.FailCatch:
                    missionMessage.text = gameData.gameNotify[5];
                    break;
            }
            
        }

        public void TakePicture()
        {
            ShowPhotoPanel(photoButtonPanel, false);
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
            _image.sprite = screenShot;

            if (currentType == TypeFlag.PictureType.ARtype)
            {
                ShowPhotoPanel(ARpicturePanel, true);
            }
            else
            {
                ShowPhotoPanel(findAnimalPanel, true);
            }   

            _camera.targetTexture = null;
        }

        private void ShowPhotoPanel(CanvasGroup canvasGroup, bool isShow)
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
