using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class EggModal : Modal
    {
        public Button StartButton;

        [SerializeField]
        private Button BackButton, ARButton;

        [SerializeField]
        private RectTransform time1, time2;

        [SerializeField]
        private Image eggImage;

        [SerializeField]
        private GameObject eggAnimation;

        [SerializeField]
        private Sprite[] dinosaurBabySprites;

        [SerializeField]
        private Sprite[] feedDinosaurSprites;

        [SerializeField]
        private Sprite[] feedDinosaurPressSprites;

        public static int dinosaurIndex;
        public static bool startMachine;

        private bool backAnimation;

        private void Awake()
        {
         /*   
            if (PlayerPrefs.HasKey("dinosaurBaby"))
            {
                StartButton.interactable = false;
                ARButton.interactable = true;
            }
            */
            BackButton.onClick.AddListener(() =>
            {
                Modals.instance.CloseModal(); // TODO error?
            });

            ARButton.onClick.AddListener(() =>
            {
                ARModal arModal = Modals.instance.GetModel<ARModal>();
                var SpriteState = arModal.feedButton.spriteState;
                arModal.feedImage.sprite = feedDinosaurSprites[PlayerPrefs.GetInt("dinosaurBaby")];
                SpriteState.pressedSprite = feedDinosaurPressSprites[PlayerPrefs.GetInt("dinosaurBaby")];

                Modals.instance.OpenAR(PlayerPrefs.GetInt("dinosaurBaby"), TypeFlag.ARObjectType.DinosaurlBaby);
                //Modals.instance.OpenAR(dinosaurIndex, TypeFlag.ARObjectType.DinosaurlBaby);
            });

            StartButton.onClick.AddListener(() =>
            {
                dinosaurIndex = Random.Range(0,3);
                PlayerPrefs.SetInt("dinosaurBaby", dinosaurIndex);
                startMachine = true;
                StartButton.interactable = false;
                ARButton.interactable = true;
                backAnimation = true;
            });
        }

        public void RotateTimes()
        {
            Vector3 time1Pos = new Vector3(0, 58, 0);
            Vector3 time2Pos;

            switch (dinosaurIndex)
            {
                case (int)TypeFlag.DinosaurlsType.Brachiosaurus:
                    time2Pos = new Vector3(-36, -58, 0);
                    time1.localPosition = Vector3.Lerp(time1.localPosition, time1Pos, 0.04f);
                    time2.localPosition = Vector3.Lerp(time2.localPosition, time2Pos, 0.08f);
                    Debug.Log("0");
                    if (time2.localPosition.x >= -36 && time2.localPosition.x <= -35) { startMachine = false;}
                    break;
                case (int)TypeFlag.DinosaurlsType.Triceratop:
                    time2Pos = new Vector3(44, -58, 0);
                    time1.localPosition = Vector3.Lerp(time1.localPosition, time1Pos, 0.04f);
                    time2.localPosition = Vector3.Lerp(time2.localPosition, time2Pos, 0.08f);
                    Debug.Log("1");
                    if (time2.localPosition.x <= 44 && time2.localPosition.x >= 43) { startMachine = false;}
                    break;
                case (int)TypeFlag.DinosaurlsType.TRex:
                    time2Pos = new Vector3(-36, -58, 0);
                    time1.localPosition = Vector3.Lerp(time1.localPosition, time1Pos, 0.04f);
                    time2.localPosition = Vector3.Lerp(time2.localPosition, time2Pos, 0.08f);
                    Debug.Log("2");

                    if (time2.localPosition.x >= -36 && time2.localPosition.x <= -35) { startMachine = false;}
                    break;
                
            }
            
        }

        private void Update()
        {
            if (!startMachine) return;
            
            RotateTimes();

            if(backAnimation) { StartCoroutine(EggToDinosaurBaby());}
        }

        private IEnumerator EggToDinosaurBaby()
        {
            backAnimation = false;
            
            yield return new WaitForSeconds(1.5f);

            eggAnimation.SetActive(true);

            yield return new WaitForSeconds(6f);
            eggAnimation.SetActive(false);
            eggImage.sprite = dinosaurBabySprites[dinosaurIndex];
        }
    }
}
