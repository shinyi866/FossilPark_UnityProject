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
        public Text txt1;
        public ARTrackedImageManager ARTrackedImage;
        public ARPlaneManager planeManager;
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
        // AR place
        private Vector2 touchPosition = default;
        private Camera _camera;

        private float ccidWeight = 0.0f;
        private bool isSetWeight;

        private bool isGameStart;

        private Transform showARfood;
        private ARGameModal modal;
        private ARPlane currentARPlane;
        private int missionIndex = 8;
        private int currentIndex = 0;
        private TypeFlag.DinosaurlsType dinosaurlsType;

        public void Init()
        {
            _camera = CameraCtrl.instance.GetCurrentCamera();
            planeManager.enabled = true;
            planeManager.planesChanged += PlaneChange;

            foreach (var b in dinosaurlScenes) { b.SetActive(false); }

            // setup all game objects in dictionary
            foreach (GameObject food in foodGameObject)
            {
                GameObject newARObject = Instantiate(food, Vector3.zero, Quaternion.identity);
                newARObject.name = food.name;
                arObjects.Add(food.name, newARObject);
                newARObject.SetActive(false);
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

            dinosaurlScenes[index].SetActive(true);
            foodButton[index].interactable = false;

            isEat = false;
        }

        // AR Plane Track
        private void PlaneChange(ARPlanesChangedEventArgs args)
        {
            if (args.added != null && currentARPlane == null)
            {
                modal.ShowModal(missionIndex, TypeFlag.ARGameType.Game8);

                ARPlane aRPlane = args.added[0];
                currentARPlane = aRPlane;
                txt1.text = "1aRPlane " + aRPlane.transform.position;

                dinosaurlScenes[currentIndex].SetActive(true);
                dinosaurlScenes[currentIndex].transform.position = new Vector3(dinosaurlScenes[currentIndex].transform.position.x, aRPlane.transform.position.y, dinosaurlScenes[currentIndex].transform.position.z);                               
            }
            else if(args.added != null && currentARPlane != null)
            {
                if (args.added[0].transform.position.y < currentARPlane.transform.position.y)
                {
                    txt1.text = "2aRPlane " + args.added[0].transform.position;
                    currentARPlane = args.added[0];
                    dinosaurlScenes[currentIndex].transform.position = new Vector3(dinosaurlScenes[currentIndex].transform.position.x, args.added[0].transform.position.y, dinosaurlScenes[currentIndex].transform.position.z);
                }
            }
            else
            {
                modal.text.text = "請掃描周遭地面";
            }
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
            //txt.text = imageNmae;

            if (dinosaurlScenes != null)
            {
                GameObject showARObject = arObjects[imageNmae];
                showARObject.SetActive(true);
                showARObject.transform.position = imagePosition;
                showARfood = arObjects[imageNmae].GetComponent<Transform>();
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
            //Transform showARfood = arObjects["grass"].GetComponent<Transform>();

            Vector3 forward = dinosaurs[currentIndex].transform.TransformDirection(Vector3.forward);
            Vector3 testToFood = testTarget.position - dinosaurs[currentIndex].transform.position;
            Vector3 arToFood;

            float dotResult = 0;

            dinosaurlsType = GetCurrentType(currentIndex);

            if (TestMode)
            {
                Vector3 lookAtTarget = new Vector3(testTarget.position.x, dinosaurs[currentIndex].transform.position.y, testTarget.position.z);
                dinosaurs[currentIndex].transform.LookAt(lookAtTarget);                
                dotResult = Vector3.Dot(forward, testToFood);
                dinosaurs[currentIndex].GetComponent<CCDIK>().solver.target = testTarget;
            }
            else
            {
                if(showARfood != null)
                {
                    arToFood = showARfood.position - dinosaurs[currentIndex].transform.position;
                    dinosaurs[currentIndex].GetComponent<CCDIK>().solver.target = showARfood;
                    Vector3 lookAtTarget = new Vector3(arToFood.x, dinosaurs[currentIndex].transform.position.y, arToFood.z);
                    dinosaurs[currentIndex].transform.LookAt(lookAtTarget);
                    dotResult = Vector3.Dot(forward, arToFood);
                }
            }

            Debug.Log("dotResult " + dotResult);
            txt.text = dotResult.ToString();
            // brachiosa: > 6.5            

            switch (dinosaurlsType)
            {
                case TypeFlag.DinosaurlsType.Brachiosaurus:
                    break;
                case TypeFlag.DinosaurlsType.TRex:
                    if (dotResult > 6.5)
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", true);
                    }
                    else
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", false);
                        //Debug.Log("not walk");

                        if (dotResult < 5.3)//1.8
                        {
                            Debug.Log("Can Eat");

                            if (isEat)
                            {
                                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", false);
                                Debug.Log("End Eat");
                            }
                            else
                            {
                                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", true);
                                Debug.Log("Eat");
                            }
                        }
                        else
                        {
                            Debug.Log("Not Eat");
                        }
                    }
                    break;
                case TypeFlag.DinosaurlsType.Triceratop:
                    if (dotResult > 3)
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", true);
                        Debug.Log("Tricerato");
                    }
                    else
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", false);
                        //Debug.Log("not walk");

                        if (dotResult < 2.5)//1.8
                        {
                            Debug.Log("Can Eat");

                            if (isEat)
                            {
                                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", false);
                                Debug.Log("End Eat");
                            }
                            else
                            {
                                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", true);
                                Debug.Log("Eat");
                            }
                        }
                        else
                        {
                            Debug.Log("Not Eat");
                        }
                    }
                    break;

            }
            
        }

        private void BrachiosaurusEat(float dotResult)
        {
            if (dotResult > 6.5)
            {
                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", true);
                //Debug.Log("walk");
            }
            else
            {
                dinosaurs[currentIndex].GetComponent<Animator>().SetBool("walk", false);
                //Debug.Log("not walk");

                if (dotResult < 5.3)//1.8
                {
                    Debug.Log("Can Eat");

                    if (isEat)
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", false);
                        dinosaurs[0].GetComponent<CCDIK>().solver.target = null;
                        ccidWeight -= Time.deltaTime * 0.03f;
                        dinosaurs[0].GetComponent<CCDIK>().solver.SetIKPositionWeight(ccidWeight);
                        Debug.Log("End Eat");
                    }
                    else
                    {
                        dinosaurs[currentIndex].GetComponent<Animator>().SetBool("eat", true);
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

            //if (showARfood != null) //AR test
                TargetDirection();
            
        }

        private TypeFlag.DinosaurlsType GetCurrentType(int index)
        {
            return (TypeFlag.DinosaurlsType)index;
        }
    }
}
