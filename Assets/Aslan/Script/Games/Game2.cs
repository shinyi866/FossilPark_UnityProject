using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Game2 : Game
    {
        [SerializeField]
        private ARPlaneManager planeManager;
        [SerializeField]
        private bool TestMode;

        public System.Action<bool> gameOverEvent;

        private int missionIndex = 2;

        private bool isARGameStart;
        private bool isUnARGameStart;
        private int successTimes;
        private ARGameModal modal;

        public void Init()
        {
            if (TestMode) return;

            Object.SetActive(false);
        }        

        public void GameStart(bool isARsupport)
        {
            if (isARsupport)
            {
                isARGameStart = true;
                SupportAR();
            }
            else
            {
                isUnARGameStart = true;
                //UnsupportAR();
            }

        }

        private void SupportAR()
        {
            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game2);

            planeManager.enabled = true;
            planeManager.planesChanged += PlaneChange;
        }
        
        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        // AR Plane Track
        private void PlaneChange(ARPlanesChangedEventArgs args)
        {
            if (args.added != null)
            {
                ARPlane aRPlane = args.added[0];

                Object.SetActive(true);
                Object.transform.position = new Vector3(Object.transform.position.x, aRPlane.transform.position.y - 1.5f, Object.transform.position.z);
            }
            else
            {
                modal.text.text = "請掃描周遭地面";
            }
        }

        private void UnsupportAR()
        {
            Debug.Log("unsupport AR");
        }
        
        private void Update()
        {
            if (isARGameStart)
            {
                isARGameStart = false;
                Debug.Log("AR Game");
            }

            if (isUnARGameStart && successTimes == 3)
            {
                //===modal.PictureButton.interactable = false;
                //modal.ConfirmButton.onClick.AddListener(() =>
                //{
                    isUnARGameStart = false;
                    GameResult(true);
                //});
            }

        }
    }
}
