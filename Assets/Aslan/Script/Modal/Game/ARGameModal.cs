using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ARGameModal : Modal
    {
        [SerializeField]
        private Text text;

        public Game2Panel game2Panel;
        public Game3Panel game3Panel;
        public Game6Panel game6Panel;
        public Game8Panel game8Panel;

        private CanvasGroup[] GameCanvasGroups;
        private GameDialogData data;

        private void Awake()
        {
            data = MainApp.Instance.database;
            GameCanvasGroups = new CanvasGroup[] { game2Panel.canvasGroup, game3Panel.canvasGroup, game6Panel.canvasGroup, game8Panel.canvasGroup };
        }

        public void ShowModal(int index, TypeFlag.ARGameType type)
        {
            var supportAR = MainApp.Instance.isARsupport;
            var gameData = data.m_Data[index];

            foreach (var c in GameCanvasGroups) { ShowCanvas(c, false); }

            text.text = gameData.gameNotify;

            switch(type)
            {
                case TypeFlag.ARGameType.Game2:
                    ShowCanvas(game2Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game3:
                    //===if (!supportAR) { unSupportView.SetActive(true); }
                    ShowCanvas(game3Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game6:
                    ShowCanvas(game6Panel.canvasGroup, true);
                    break;
                case TypeFlag.ARGameType.Game8:
                    ShowCanvas(game8Panel.canvasGroup, true);
                    break;
            }
        }

        private void ShowCanvas(CanvasGroup canvasGroup,bool isShow)
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
public class Game2Panel
{
    public CanvasGroup canvasGroup;
    public Image[] images;
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
}

