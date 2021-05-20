using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View
{
    public class InfoModal : Modal
    {
        [Range(0, 25)]
        public int speed = 15;
        [Header("Info Item")]
        [SerializeField]
        private AnimalItemObj animalItemObj;

        [SerializeField]
        private Image MainImage;

        [SerializeField]
        private Image NameImage;

        [SerializeField]
        private Text InfoText;

        [Header("Buttons")]
        [SerializeField]
        private Button BackButton;

        [SerializeField]
        private Button ARButton;

        [SerializeField]
        private GameObject[] nextGameObject;

        private bool dinosaurlView;
        private Button beforeButton;
        private Button afterButton;
        private Sprite[] currentSprites;

        private int currentIndex;
        private TypeFlag.ARObjectType currentType;

        private void Awake()
        {
            beforeButton = nextGameObject[0].GetComponent<Button>();
            afterButton = nextGameObject[1].GetComponent<Button>();

            BackButton.onClick.AddListener(() =>
            {
                Modals.instance.CloseModal(); // TODO error?
                currentSprites = null;

                if (dinosaurlView)
                {
                    Modals.instance.OpenModal<DinosaurlModal>();
                    iBeaconMissionSetting.Instance.isEnterGame = false;
                    dinosaurlView = false;                    
                }
            });

            ARButton.onClick.AddListener(() =>
            {
                var arIndex = currentIndex + 2;
                
                if(MainApp.Instance.isARsupport)
                    Modals.instance.OpenAR(arIndex, currentType);
                else
                    Modals.instance.OpenNotSupportAR(arIndex, currentType);
                    
                iBeaconMissionSetting.Instance.isEnterGame = true;

                currentSprites = null;
            });
        }

        public void ShowInfo(int index, TypeFlag.InfoType type)
        {
            currentIndex = index;
            
            switch (type)
            {
                case TypeFlag.InfoType.Animal:
                    foreach (var gameObject in nextGameObject) { gameObject.SetActive(false); }
                    currentSprites = animalItemObj.AnimalItems[index].MainImage;
                    NameImage.sprite = animalItemObj.AnimalItems[index].NameImage;
                    InfoText.text = animalItemObj.AnimalItems[index].text;

                    currentType = TypeFlag.ARObjectType.Animals;
                    break;

                case TypeFlag.InfoType.Dinosaurl:
                    foreach (var gameObject in nextGameObject) { gameObject.SetActive(true); }
                    currentSprites = animalItemObj.DinosaurlItems[index].MainImage;
                    NameImage.sprite = animalItemObj.DinosaurlItems[index].NameImage;
                    InfoText.text = animalItemObj.DinosaurlItems[index].text;

                    NextClick(index);
                    dinosaurlView = true;

                    currentType = TypeFlag.ARObjectType.Dinosaurls;
                    break;
            }
        }

        private void NextClick(int index)
        {
            beforeButton.onClick.AddListener(() =>
            {
                if (index <= 0) return;
                index--;
                currentIndex = index;
                currentSprites = animalItemObj.DinosaurlItems[index].MainImage;
                NameImage.sprite = animalItemObj.DinosaurlItems[index].NameImage;
                InfoText.text = animalItemObj.DinosaurlItems[index].text;
            });

            afterButton.onClick.AddListener(() =>
            {
                if (index >= animalItemObj.DinosaurlItems.Length - 1) return;
                index++;
                currentIndex = index;
                currentSprites = animalItemObj.DinosaurlItems[index].MainImage;
                NameImage.sprite = animalItemObj.DinosaurlItems[index].NameImage;
                InfoText.text = animalItemObj.DinosaurlItems[index].text;
            });
        }

        private void Update()
        {
            if (currentSprites == null || currentSprites.Length < 1) return;
            
            MainImage.sprite = currentSprites[(int)(Time.time* speed) % currentSprites.Length];
        }
    }
}
