using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ARGameModal : Modal
    {
        public CanvasGroup[] GameCanvasGroup; // sort: mission3, mission5, mission6, mission7

        [Header("Mission3")]
        public GameObject unSupportView;
        public Text countText;
        public Text getFruitText;
        public Button leftButton;
        public Button rightButton;

        [Header("Mission8")]
        public Button[] foodButton; // 0: grass, 1:meat, 2:fish

        public void ShowModal(TypeFlag.ARGameType type)
        {
            var supportAR = MainApp.Instance.isARsupport;

            foreach (var c in GameCanvasGroup) { ShowCanvas(c, false); }

            switch(type)
            {
                case TypeFlag.ARGameType.Game3:
                    if (!supportAR) { unSupportView.SetActive(true); }
                    ShowCanvas(GameCanvasGroup[0], true);
                    break;
                case TypeFlag.ARGameType.Game5:
                    ShowCanvas(GameCanvasGroup[1], true);
                    break;
                case TypeFlag.ARGameType.Game6:
                    ShowCanvas(GameCanvasGroup[2], true);
                    break;
                case TypeFlag.ARGameType.Game7:
                    ShowCanvas(GameCanvasGroup[3], true);
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

