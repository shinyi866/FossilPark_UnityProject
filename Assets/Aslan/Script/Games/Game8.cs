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
        public Text txt1;
        public Text txt3;
        public ARTrackedImageManager ARTrackedImage;
        public ARPlaneManager planeManager;
        public GameObject[] foodGameObject; // 0:plant2, 1:plant, 2:meat
        public GameObject[] foodinMouth;
        public GameObject[] dinosaurlScenes; // 0:brachiosaurus, 1:triceratop 2:TRex
        public GameObject[] dinosaurs; // 0:brachiosaurus, 1:triceratop 2:TRex
        public Transform[] dinosaursTransform;
        public Transform testTarget;
        public bool TestMode;

        public static bool isEat;
        //public static bool resetEatFood;
        public System.Action<bool> gameOverEvent;

        private ARTrackedImage _trackImage;
        private Button[] foodButton = new Button[3]; // 0:brachiosaurus, 1:triceratop 2:TRex
        private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
        // AR place
        private Vector2 touchPosition = default;

        private string currentImageName;
        private int missionIndex = 8;
        private int currentIndex = 0;
        private double time = 3;
        private float ccidWeight = 0.0f;
        private bool isGameStart;

        public static GameObject currentDinosaurl;
        private GameObject currentFood;
        private GameObject showARfood;
        private ARGameModal modal;
        private ARPlane placeObject;
        private TriggerARObject triggerARObject;
        private TypeFlag.DinosaurlsType dinosaurlsType;        

        public void Init()
        {
            foreach (var b in dinosaurlScenes) { b.SetActive(false); }

            // setup all game objects in dictionary
            for(int i = 0; i < foodGameObject.Length; i++)
            {
                GameObject newARObject = Instantiate(foodGameObject[i], foodGameObject[i].transform.position, foodGameObject[i].transform.rotation);
                newARObject.name = foodGameObject[i].name;
                arObjects.Add(foodGameObject[i].name, newARObject);
                newARObject.SetActive(false);

                dinosaurs[i].GetComponent<CCDIK>().solver.target = newARObject.transform; // set dinosaurl eat food
            }

            modal = GameModals.instance.OpenModal<ARGameModal>();
            modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game8);

            ButtonSetUp();
        }

        public void GameStart()
        {
            SwitchDinosaurlScene((int)TypeFlag.DinosaurlsType.Brachiosaurus);
            isGameStart = true;
        }

        private void ResetDirection()
        {
            if (time > 0)
            {
                Compass.Instance.SetUp(Object, 275);
                time -= Time.deltaTime;
            }
        }

        private void ButtonSetUp()
        {
            for (int i = 0; i < foodButton.Length; i++) { foodButton[i] = modal.game8Panel.foodButtons[i]; }
            for (int i = 0; i < foodButton.Length; i++)
            {
                int closureIndex = i;
                foodButton[closureIndex].onClick.AddListener(() =>
                {
                    currentIndex = closureIndex;
                    SwitchDinosaurlScene(closureIndex);
                });
            }
        }

        private void SwitchDinosaurlScene(int index)
        {
            foreach (var o in dinosaurlScenes) { o.SetActive(false); }
            foreach (var b in foodButton) { b.interactable = true; }
            foreach(var f in foodinMouth) { f.SetActive(false); }

            ResetDinosaurls();
            currentDinosaurl = dinosaurs[index];
            dinosaurlScenes[index].SetActive(true);
            foodButton[index].interactable = false;
            isEat = false;

            EggModal.dinosaurIndex = index;
            Modals.instance.GetModel<EggModal>().RotateTimes();
            EggModal.startMachine = true;
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
            currentImageName = trackImage.referenceImage.name;
            var imagePosition = trackImage.transform.position;

            if (dinosaurlScenes != null)
            {
                if(currentImageName != "ticket")
                {
                    GameObject showARObject = arObjects[currentImageName];
                    showARObject.SetActive(true);
                    showARObject.transform.position = imagePosition;
                    //showARObject.transform.rotation = trackImage.transform.rotation;
                    showARfood = arObjects[currentImageName];
                    //resetEatFood = true;

                    foreach (GameObject b in arObjects.Values)
                    {
                        Debug.Log($"Show in arObjects.Values: {b.name}");
                        if (b.name != currentImageName)
                        {
                            b.SetActive(false);
                        }
                    }

                    if (isEat) { showARObject.SetActive(false); }
                }
                else
                {
                    currentImageName = foodGameObject[currentIndex].name;
                    GameObject showARObject = arObjects[currentImageName];
                    showARObject.SetActive(true);
                    showARObject.transform.position = imagePosition;

                    showARfood = arObjects[currentImageName];
                    //resetEatFood = true;

                    foreach (GameObject b in arObjects.Values)
                    {
                        Debug.Log($"Show in arObjects.Values: {b.name}");
                        if (b.name != currentImageName)
                        {
                            b.SetActive(false);
                        }                        
                    }

                    if (isEat) { showARObject.SetActive(false); }
                }
            }
        }

        // Dinosaurl find need walk or not
        private void TargetDirection()
        {
            Vector3 forward = currentDinosaurl.transform.TransformDirection(Vector3.forward);
            Vector3 testToFood = testTarget.position - currentDinosaurl.transform.position;
            Vector3 arToFood;

            float dotResult = 0;

            dinosaurlsType = GetCurrentType(currentIndex);
            
            if (TestMode)
            {
                //resetEatFood = true;
                Vector3 lookAtTarget = new Vector3(testTarget.position.x, currentDinosaurl.transform.position.y, testTarget.position.z);
                currentDinosaurl.transform.LookAt(lookAtTarget);                
                dotResult = Vector3.Dot(forward, testToFood);
                currentDinosaurl.GetComponent<CCDIK>().solver.target = testTarget;
                switch (dinosaurlsType)
                {
                    case TypeFlag.DinosaurlsType.Brachiosaurus:
                        DinosaursEat(dotResult, 5f, 4.95f, 4f, 0.04f);
                        break;
                    case TypeFlag.DinosaurlsType.TRex:
                        DinosaursEat(dotResult, 2.7f, 2.67f, 1.6f, 0.4f);
                        break;
                    case TypeFlag.DinosaurlsType.Triceratop:
                        DinosaursEat(dotResult, 2.5f, 2.45f, 1.9f, 0.05f);
                        break;
                }
                Debug.Log("TestMode ");
            }
            else
            {
                if(showARfood != null)
                {
                    Vector3 lookAtTarget = new Vector3(showARfood.transform.position.x, currentDinosaurl.transform.position.y, showARfood.transform.position.z);
                    currentDinosaurl.transform.LookAt(lookAtTarget);
                    arToFood = showARfood.transform.position - currentDinosaurl.transform.position;
                    dotResult = Vector3.Dot(forward, arToFood);
                    //Debug.Log("resetEatFood " + resetEatFood);

                    switch (dinosaurlsType)
                    {
                        case TypeFlag.DinosaurlsType.Brachiosaurus:
                            DinosaursEat(dotResult, 5f, 4.97f, 4f, 0.04f);
                            break;
                        case TypeFlag.DinosaurlsType.TRex:
                            DinosaursEat(dotResult, 2.7f, 2.67f, 1.6f, 0.4f);
                            break;
                        case TypeFlag.DinosaurlsType.Triceratop:
                            DinosaursEat(dotResult, 2.5f, 2.45f, 1.9f, 0.05f);
                            break;
                    }                    
                }
            }

            Debug.Log("dotResult " + dotResult);
            txt1.text = "dotResult: " + dotResult;
        }                  

        private void DinosaursEat(float dotResult, float walkDotResult, float eatDotResult, float limitResult, float trackSpeed)
        {
            ccidWeight = currentDinosaurl.GetComponent<CCDIK>().solver.GetIKPositionWeight();
            
            if (dotResult > walkDotResult && arObjects[currentImageName].activeInHierarchy)  //  && arObjects[currentImageName].activeInHierarchy)  // 
            {
                currentDinosaurl.GetComponent<Animator>().SetBool("walk", true);                
            }

            if(dotResult < walkDotResult && arObjects[currentImageName].activeInHierarchy)
            {
                currentDinosaurl.GetComponent<Animator>().SetBool("walk", false);

                if (dotResult < eatDotResult && dotResult > limitResult)
                {
                    Debug.Log("Can Eat");
                    txt3.text = "Can Eat";

                    if(isEat)
                    {
                        //currentDinosaurl.GetComponent<Animator>().SetBool("eat", true);
                        ccidWeight -= Time.deltaTime * trackSpeed;
                        currentDinosaurl.GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                        txt3.text = $"Eat, ccid: {ccidWeight}";
                        Debug.Log("Eat");
                    }
                    else
                    {
                        if (ccidWeight < 0.6)
                        {
                            ccidWeight += Time.deltaTime * trackSpeed;
                            currentDinosaurl.GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                        }
                    }
                }
            }

            if(!isEat && !arObjects[currentImageName].activeInHierarchy)
            {
                currentDinosaurl.GetComponent<Animator>().SetBool("eat", false);
                //currentDinosaurl.GetComponent<CCDIK>().solver.target = null;
                ccidWeight -= Time.deltaTime * trackSpeed;
                currentDinosaurl.GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);

                txt3.text = $"End Eat, ccid: {ccidWeight}";
            }
        }


        private void ResetDinosaurls()
        {
            for(int i = 0; i < dinosaurs.Length; i++)
            {
                for(int j = 0; j < dinosaursTransform.Length; j++)
                {
                    if(i == j)
                    {
                        dinosaurs[i].transform.position = dinosaursTransform[j].transform.position;
                        dinosaurs[i].transform.rotation = dinosaursTransform[j].transform.rotation;
                        dinosaurs[i].GetComponent<CCDIK>().solver.SetIKPositionWeight(0);
                    }
                }
            }
        }

        private void Update()
        {
            if (!isGameStart) return;

            ResetDirection();

            if (!TestMode)
            {
                if (currentImageName == null) return; //AR mode
                TargetDirection();
            }
            else
            {
                TargetDirection();
            }
        }
        
        private TypeFlag.DinosaurlsType GetCurrentType(int index)
        {
            return (TypeFlag.DinosaurlsType)index;
        }
        
    }
}
