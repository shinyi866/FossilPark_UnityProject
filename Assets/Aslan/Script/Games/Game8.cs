using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace GameMission
{
    public class Game8 : Game
    {
        public Text txt;
        public ARTrackedImageManager ARTrackedImage;
        public GameObject[] foodGameObject; // 0:grass, 1:meat, 2:fish
        public GameObject[] dinosaurlScenes; // 0:grass, 1:meat, 2:fish
        public GameObject[] dinosaurs; // 0:grass, 1:meat, 2:fish
        public Transform[] topBons;// 0:grass, 1:meat, 2:fish
        public Transform testTarget;
        public bool Test;

        public System.Action<bool> gameOverEvent;

        private ARTrackedImage _trackImage;
        //private Transform Target;
        private Button[] foodButton = new Button[3]; // 0:grass, 1:meat, 2:fish
        private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
        private bool isGameStart;

        public void Init()
        {
            foreach(var b in dinosaurlScenes) { b.SetActive(false); }

            // setup all game objects in dictionary
            foreach (GameObject food in foodGameObject)
            {
                GameObject newARObject = Instantiate(food, Vector3.zero, Quaternion.identity);
                newARObject.name = food.name;
                arObjects.Add(food.name, newARObject);
            }

            ButtonSetUp();
        }

        public void GameStart()
        {
            SwitchDinosaurlScene(0);
            isGameStart = true;
        }

        private void ButtonSetUp()
        {
            for (int i = 0; i < foodButton.Length; i++) { foodButton[i] = GameModals.instance.GetModal<ARGameModal>().foodButton[i]; }
            for (int i = 0; i < foodButton.Length; i++)
            {
                int closureIndex = i;
                foodButton[closureIndex].onClick.AddListener(() => { SwitchDinosaurlScene(closureIndex); });
            }
        }

        private void SwitchDinosaurlScene(int index)
        {
            foreach (var o in dinosaurlScenes) { o.SetActive(false); }
            foreach (var b in foodButton) { b.interactable = true; }

            dinosaurlScenes[index].SetActive(true);
            foodButton[index].interactable = false;
        }

        // AR Image Track
        private void OnEnable()
        {
            ARTrackedImage.trackedImagesChanged += OnTrackedImagesStart;
        }

        private void OnDisable()
        {
            ARTrackedImage.trackedImagesChanged -= OnTrackedImagesStart;
        }

        private void OnTrackedImagesStart(ARTrackedImagesChangedEventArgs eventArgs)
        {

            foreach (ARTrackedImage trackImage in eventArgs.added)
            {
                UpdateARImage(trackImage);
            }

            foreach (ARTrackedImage trackImage in eventArgs.updated)
            {
                UpdateARImage(trackImage);
            }

            foreach (ARTrackedImage trackImage in eventArgs.removed)
            {
                arObjects[trackImage.referenceImage.name].SetActive(false);
            }
        }

        private void UpdateARImage(ARTrackedImage trackImage)
        {
            var imageNmae = trackImage.referenceImage.name;
            var imagePosition = trackImage.transform.position;
            txt.text = imageNmae;

            if (dinosaurlScenes != null)
            {
                GameObject showARObject = arObjects[imageNmae];
                showARObject.SetActive(true);
                showARObject.transform.position = imagePosition;
                
                foreach (GameObject b in arObjects.Values)
                {
                    Debug.Log($"Show in arObjects.Values: {b.name}");
                    if (b.name != imageNmae)
                    {
                        b.SetActive(false);
                    }
                }
            }
        }

        // Dinosaurl find target to eat
        private void TargetDirection()
        {
            var ARobject = dinosaurs[0];
            Transform showARfood = arObjects["grass"].GetComponent<Transform>();
            float targetDirection;

            if (Test)
            {
                targetDirection = (topBons[0].position - testTarget.position).sqrMagnitude;
            }
            else
            {
                targetDirection = (topBons[0].position - showARfood.position).magnitude;//.sqrMagnitude;
            }
            
            float compeleteLength = 0.88f;
            var lengthPow = Mathf.Pow(compeleteLength, 2);
            var touchTarget = lengthPow - targetDirection;
            Debug.Log("targetDirection " + targetDirection);
            //Debug.Log("lengthPow " + lengthPow);
            Debug.Log("touchTarget " + touchTarget);

            if (touchTarget < 1.4 && touchTarget > 0.8)
            {
                Debug.Log("can touch ");
                ARobject.GetComponent<Animator>().SetBool("walk", false);
            }
            else
            {
                Debug.Log("walk ");
                ARobject.GetComponent<Animator>().SetBool("walk", true);
            }
        }

        private void LateUpdate()
        {
            if (!isGameStart) return;

            TargetDirection();

            if (Input.GetKeyDown(KeyCode.K))
            {
                dinosaurs[0].GetComponent<Animator>().SetBool("walk", true);
            }

        }
    }
}
