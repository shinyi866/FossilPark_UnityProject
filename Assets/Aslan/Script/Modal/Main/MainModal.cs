using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMission;

namespace View
{
    public class MainModal : Modal
    {
        [Header("Buttons")]
        [SerializeField]
        private Button[] animalButtons;
        [SerializeField]
        private Button dinosaurButton;
        [SerializeField]
        private Button eggButton;

        [SerializeField]
        private Button[] missionButtons;

        [Header("Image")]
        public Image scoreImage;
        [SerializeField]
        private Image clockImage;
        [SerializeField]
        private Sprite[] clockSprite;

        private int clockOutSide = 0;

        public void StarMainView()
        {
            MainButtonClick();
            MissionsButtonClick();
        }

        public void GetBackAnimal(int index)
        {
            Debug.Log("get back");
            var buttonIndex = index - 2;
            clockOutSide++;

            if (buttonIndex >= 0 && buttonIndex < animalButtons.Length)
            {
                animalButtons[buttonIndex].GetComponent<Image>().enabled = true;
                ChangeClock();
            }
        }

        private void MainButtonClick()
        {
            for (int i = 0; i < animalButtons.Length; i++)
            {
                int closureIndex = i;
                animalButtons[closureIndex].onClick.AddListener(() =>
                {
                    var modal = Modals.instance.OpenModal<InfoModal>();
                    modal.ShowInfo(closureIndex, TypeFlag.InfoType.Animal);
                });
            }

            dinosaurButton.onClick.AddListener(() => {
                var modal = Modals.instance.OpenModal<DinosaurlModal>();
                modal.Setup();
            });

            eggButton.onClick.AddListener(() => {
                Modals.instance.OpenModal<EggModal>();
            });
        }

        private void MissionsButtonClick()
        {
            for (int i = 0; i < missionButtons.Length; i++)
            {
                int closureIndex = i;
                missionButtons[closureIndex].onClick.AddListener(() =>
                {
                    Debug.Log("closureIndex " + closureIndex);
                    GameMissions.instance.ShowMission(closureIndex);
                });
            }
        }

        private void ChangeClock()
        {
            Debug.Log("clockOutSide " + clockOutSide);
            if (clockOutSide == 0) { clockImage.sprite = clockSprite[0]; }
            if (clockOutSide == 2) { clockImage.sprite = clockSprite[1]; }
            if (clockOutSide == 4) { clockImage.sprite = clockSprite[2]; }
            if (clockOutSide == 6) { clockImage.sprite = clockSprite[3]; }
        }
    }
}
