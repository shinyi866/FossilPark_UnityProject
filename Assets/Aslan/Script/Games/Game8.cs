using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using RootMotion.FinalIK;

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
        public bool TestMode;

        [HideInInspector]
        public bool isEat;

        // vector to caculate food direction
        //public Transform centerPoint;

        public System.Action<bool> gameOverEvent;

        private ARTrackedImage _trackImage;
        //private Transform Target;
        private Button[] foodButton = new Button[3]; // 0:grass, 1:meat, 2:fish
        private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

        private float ccidWeight = 0.0f;
        private bool isSetWeight;

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

            isEat = false;
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
                dinosaurs[0].GetComponent<CCDIK>().solver.target = showARObject.transform;

                foreach (GameObject b in arObjects.Values)
                {
                    Debug.Log($"Show in arObjects.Values: {b.name}");
                    if (b.name != imageNmae)
                    {
                        b.SetActive(false);
                    }

                    if (isEat) { showARObject.SetActive(false); }
                }
            }
        }

        // Dinosaurl find need walk or not
        private void TargetDirection()
        {
            Transform showARfood = arObjects["grass"].GetComponent<Transform>();

            Vector3 forward = dinosaurs[0].transform.TransformDirection(Vector3.forward);
            Vector3 testToFood = testTarget.position - dinosaurs[0].transform.position;
            Vector3 arToFood = showARfood.position - dinosaurs[0].transform.position;

            float dotResult;
            Vector3 lookAtTarget = new Vector3(testTarget.position.x, dinosaurs[0].transform.position.y, testTarget.position.z);

            dinosaurs[0].transform.LookAt(lookAtTarget);

            if (TestMode)
            {
                dinosaurs[0].GetComponent<CCDIK>().solver.target = testTarget;
                dotResult = Vector3.Dot(forward, testToFood);
            }
            else
            {
                dotResult = Vector3.Dot(forward, arToFood);
            }

            //Debug.Log("dotResult " + dotResult);

            if(dotResult > 2.5)
            {
                dinosaurs[0].GetComponent<Animator>().SetBool("walk", true);
                Debug.Log("walk");
            }
            else
            {
                dinosaurs[0].GetComponent<Animator>().SetBool("walk", false);
                Debug.Log("not walk");

                if (dotResult > 1.8)
                {
                    Debug.Log("Can Eat");

                    if (isEat)
                    {
                        dinosaurs[0].GetComponent<CCDIK>().solver.target = null;
                        ccidWeight -= Time.deltaTime * 0.03f;
                        dinosaurs[0].GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                        Debug.Log("End Eat");
                    }
                    else
                    {
                        ccidWeight += Time.deltaTime * 0.03f;
                        dinosaurs[0].GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                        Debug.Log("Eat");
                    }
                }
                else
                {
                    ccidWeight -= Time.deltaTime * 0.03f;
                    dinosaurs[0].GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                    Debug.Log("Not Eat");
                }
            }
        }

        private void LateUpdate()
        {
            if (!isGameStart) return;

            TargetDirection();
        }
    }
}
